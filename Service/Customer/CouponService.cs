using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Manager;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Coupons;

namespace Service.Customer;

public class CouponService(AppDbContext context)
{
    public async Task<List<CouponInCheckoutPageDto>> GetAvailableCouponsForOrderAsync(int customerId, long totalAmount)
    {
        var distinctCouponIdsQuery = context.CustomerCoupons
            .Where(cc => cc.CustomerId == customerId &&
                         cc.ExpiryDate > DateTime.UtcNow &&
                         !cc.IsUsed &&
                         (!cc.Coupon!.MinOrderAmount.HasValue || cc.Coupon.MinOrderAmount.Value <= totalAmount))
            .GroupBy(cc => cc.CouponId)
            .Select(g => g.OrderBy(cc => cc.ExpiryDate).FirstOrDefault()!.Id);
        return await context.CustomerCoupons
            .Where(cc => distinctCouponIdsQuery.Contains(cc.Id))
            .OrderBy(cc => cc.ExpiryDate)
            .ProjectToAvailableCouponForOrderDto()
            .ToListAsync();
    }

    public async Task<PagedAndSortedDto<CouponViewDto>> GetAvailableCouponsForViewAsync(int customerId,
        PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortColumn ??= nameof(CustomerCoupon.ExpiryDate);
        request.SortDirection ??= SortDirection.Ascending;

        var query = context.CustomerCoupons.Where(cc => cc.CustomerId == customerId && cc.Coupon != null);

        var filter = request.Filter;
        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim();
            query = query.WhereContainsUnaccent(keyword, cc => cc.Coupon!.Description);
        }

        if (filter.DiscountType.HasValue)
        {
            query = query.Where(cc => cc.Coupon!.DiscountType == filter.DiscountType.Value);
        }

        var totalCount = await query.CountAsync();
        if (totalCount == 0)
        {
            return new PagedAndSortedDto<CouponViewDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var dtos = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ProjectToCouponViewDto()
            .ToListAsync();

        return new PagedAndSortedDto<CouponViewDto>(dtos, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }

    public async Task<int> RemoveAllExpiredCustomerCouponsAsync() =>
        await context.CustomerCoupons.Where(cc => cc.ExpiryDate <= DateTime.UtcNow).ExecuteDeleteAsync();
}