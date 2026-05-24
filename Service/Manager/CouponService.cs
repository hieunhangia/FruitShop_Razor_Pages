using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Coupons;
using Service.DTOs;
using Service.DTOs.Manager;

namespace Service.Manager;

public class CouponService(AppDbContext context, CouponMapper mapper)
{
    public async Task<PagedAndSortedDto<Coupon>> GetAllCouponsAsync(PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortDirection ??= SortDirection.Ascending;
        request.SortColumn ??= nameof(Coupon.Id);
        var query = context.Coupons.AsNoTracking();
        var filter = request.Filter;

        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim();
            query = query.WhereContainsUnaccent(keyword, c => c.Description);
        }

        if (filter.DiscountType.HasValue)
        {
            query = query.Where(c => c.DiscountType == filter.DiscountType.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == filter.IsActive.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ToListAsync();

        return new PagedAndSortedDto<Coupon>(items, totalCount, request.PageIndex, request.PageSize, request.SortColumn,
            request.SortDirection.Value);
    }

    public async Task<CouponUpdateDto?> GetCouponByIdAsync(int id)
    {
        var query = await context.Coupons.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        if (query == null) return null;
        return mapper.ToCouponUpdateDto(query);
    }

    public async Task UpdateCouponAsync(int id, CouponUpdateDto input)
    {
        var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Id == id);
        if (coupon == null)
        {
            throw new Exception("Coupon không tồn tại");
        }

        if (input is { DiscountType: DiscountType.Percentage, DiscountValue: > 100 })
        {
            throw new Exception("Giá trị giảm giá theo phần trăm không thể lớn hơn 100%");
        }

        coupon.Description = input.Description;
        coupon.DiscountType = input.DiscountType;
        coupon.DiscountValue = input.DiscountValue;
        coupon.MaxDiscountAmount = input.MaxDiscountAmount;
        coupon.MinOrderAmount = input.MinOrderAmount;
        coupon.LoyaltyPointsCost = input.LoyaltyPointsCost;
        coupon.IsActive = input.IsActive;

        await context.SaveChangesAsync();
    }
}