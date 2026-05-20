using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Manager;
using Repository.Constants;
using Repository.Data.Extensions;
using CouponMapper = Service.DTOs.Customer.Coupon.CouponMapper;

namespace Service.Customer;

public class CouponService(AppDbContext context, CouponMapper mapper)
{
    public async Task<List<CouponInCheckoutPageDto>>
        GetAvailableCouponsForOrderAsync(int customerId, long totalAmount) => mapper.ToAvailableCouponForOrderDtoList(
        await context.CustomerCoupons.AsNoTracking()
            .Include(c => c.Coupon)
            .Where(cc =>
                cc.CustomerId == customerId && cc.ExpiryDate > DateTime.UtcNow && !cc.IsUsed &&
                (!cc.Coupon!.MinOrderAmount.HasValue || cc.Coupon.MinOrderAmount.Value <= totalAmount))
            .ToListAsync());

    public async Task<PagedAndSortedDto<CouponViewDto>> GetAvailableCouponsForViewAsync(int customerId,
        PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortColumn ??= nameof(Repository.Models.Coupons.CustomerCoupon.Id);
        request.SortDirection ??= SortDirection.Ascending;

        var query = context.CustomerCoupons.AsNoTracking()
            .Include(c => c.Coupon)
            .Where(cc =>
                cc.CustomerId == customerId &&
                cc.ExpiryDate.HasValue &&
                cc.ExpiryDate.Value > DateTime.UtcNow &&
                !cc.IsUsed &&
                cc.Coupon != null &&
                cc.Coupon.IsActive);

        var filter = request.Filter;
        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim().ToLower();
            query = query.Where(cc => cc.Coupon!.Description.ToLower().Contains(keyword));
        }

        if (filter.DiscountType.HasValue)
        {
            query = query.Where(cc => cc.Coupon!.DiscountType == filter.DiscountType.Value);
        }

        if (filter.IsActive.HasValue)
        {
            query = query.Where(cc => cc.Coupon!.IsActive == filter.IsActive.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var dtos = mapper.ToCouponViewDtoList(items);
        return new PagedAndSortedDto<CouponViewDto>(dtos, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }
}