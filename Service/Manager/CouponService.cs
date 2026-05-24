using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Coupons;
using Service.DTOs;
using Service.DTOs.Manager;

namespace Service.Manager;

public class CouponService(AppDbContext context)
{
    public async Task<PagedAndSortedDto<CouponDto>> GetAllCouponsAsync(PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortDirection ??= SortDirection.Ascending;
        request.SortColumn ??= nameof(Coupon.Id);
        var query = context.Coupons.AsQueryable();
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

        if (totalCount == 0)
        {
            return new PagedAndSortedDto<CouponDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ProjectToCouponDto()
            .ToListAsync();

        return new PagedAndSortedDto<CouponDto>(items, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn,
            request.SortDirection.Value);
    }

    public async Task<CouponUpdateDto?> GetCouponByIdAsync(int id) => await context.Coupons
        .Where(c => c.Id == id)
        .ProjectToCouponUpdateDto()
        .FirstOrDefaultAsync();

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