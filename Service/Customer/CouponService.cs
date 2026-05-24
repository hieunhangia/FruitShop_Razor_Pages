using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs;
using Service.DTOs.Customer.Coupon;
using Service.DTOs.Manager;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Coupons;
using CouponMapper = Service.DTOs.Customer.Coupon.CouponMapper;

namespace Service.Customer;

public class CouponService(AppDbContext context, CouponMapper mapper)
{
    public async Task<List<CouponInCheckoutPageDto>> GetAvailableCouponsForOrderAsync(int customerId, long totalAmount)
    {
        var availableCoupons = await context.CustomerCoupons.AsNoTracking()
            .Include(c => c.Coupon)
            .Where(cc =>
                cc.CustomerId == customerId && cc.ExpiryDate > DateTime.UtcNow && !cc.IsUsed &&
                (!cc.Coupon!.MinOrderAmount.HasValue || cc.Coupon.MinOrderAmount.Value <= totalAmount))
            .OrderBy(cc => cc.ExpiryDate)
            .ToListAsync();

        var distinctCoupons = availableCoupons
            .DistinctBy(cc => cc.CouponId)
            .ToList();

        return mapper.ToAvailableCouponForOrderDtoList(distinctCoupons);
    }

    public async Task<PagedAndSortedDto<CouponViewDto>> GetAvailableCouponsForViewAsync(int customerId,
        PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortColumn ??= nameof(CustomerCoupon.ExpiryDate);
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
            var keyword = filter.Keyword.Trim();
            query = query.WhereContainsUnaccent(keyword, cc => cc.Coupon!.Description);
        }

        if (filter.DiscountType.HasValue)
        {
            query = query.Where(cc => cc.Coupon!.DiscountType == filter.DiscountType.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ToListAsync();

        var dtos = mapper.ToCouponViewDtoList(items);
        return new PagedAndSortedDto<CouponViewDto>(dtos, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }

    public async Task<PagedAndSortedDto<CouponShopDto>> GetAllAvailableCouponsForSaleAsync(
        PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortColumn ??= nameof(Coupon.LoyaltyPointsCost);
        request.SortDirection ??= SortDirection.Ascending;

        var query = context.Coupons.AsNoTracking()
            .Where(c => c.IsActive);

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

        if (filter.StartLoyaltyPointsCost is >= 0)
        {
            query = query.Where(c => c.LoyaltyPointsCost >= filter.StartLoyaltyPointsCost.Value);
        }

        if (filter.EndLoyaltyPointsCost.HasValue)
        {
            query = query.Where(c => c.LoyaltyPointsCost <= filter.EndLoyaltyPointsCost.Value);
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ToListAsync();

        var dtos = mapper.ToCouponShopDtoList(items);

        return new PagedAndSortedDto<CouponShopDto>(dtos, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }


    public async Task<long> GetLoyaltyPoints(long customerId)
    {
        return await context.CustomerData.AsNoTracking()
            .Where(cd => cd.CustomerId == customerId)
            .Select(cd => cd.LoyaltyPoints)
            .SingleOrDefaultAsync();
    }

    public async Task BuyCoupon(long couponId, int customerId)
    {
        var coupon = await context.Coupons.FirstOrDefaultAsync(c => c.Id == couponId);
        if (coupon == null || !coupon.IsActive)
        {
            throw new Exception("Coupon không tồn tại hoặc không còn hoạt động.");
        }

        var customerData = await context.CustomerData.FirstOrDefaultAsync(cd => cd.CustomerId == customerId);
        if (customerData == null)
        {
            throw new Exception("Không tìm thấy thông tin khách hàng.");
        }

        if (customerData.LoyaltyPoints < coupon.LoyaltyPointsCost)
        {
            throw new Exception("Không đủ điểm tích lũy để mua coupon.");
        }


        customerData.LoyaltyPoints -= coupon.LoyaltyPointsCost;

        var customerCoupon = new CustomerCoupon
        {
            CustomerId = customerId,
            CouponId = coupon.Id,
            IsUsed = false,
            ExpiryDate = BusinessRuleConstants.Coupon.ExpiryDateTime
        };

        await context.CustomerCoupons.AddAsync(customerCoupon);
        await context.SaveChangesAsync();
    }
}