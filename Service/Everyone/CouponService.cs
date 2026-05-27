using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Coupons;
using Service.DTOs;
using Service.DTOs.Everyone.Coupon;
using Service.DTOs.Manager;

namespace Service.Everyone;

public class CouponService(AppDbContext context)
{
    public async Task<PagedAndSortedDto<CouponShopDto>> GetAllAvailableCouponsForSaleAsync(
        PagedAndSortedRequest<CouponFilter> request)
    {
        request.SortColumn ??= nameof(Coupon.LoyaltyPointsCost);
        request.SortDirection ??= SortDirection.Ascending;

        var query = context.Coupons.Where(c => c.IsActive);

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

        if (totalCount == 0)
        {
            return new PagedAndSortedDto<CouponShopDto>([], 0, request.PageIndex, request.PageSize,
                request.SortColumn, request.SortDirection.Value);
        }

        var dtos = await query
            .DynamicOrderBy(request.SortColumn, request.SortDirection.Value)
            .ApplyPaging(request.PageIndex, request.PageSize)
            .ProjectToCouponShopDto()
            .ToListAsync();

        return new PagedAndSortedDto<CouponShopDto>(dtos, totalCount, request.PageIndex, request.PageSize,
            request.SortColumn, request.SortDirection.Value);
    }


    public async Task<long> GetLoyaltyPoints(int customerId)
    {
        return await context.CustomerData
            .Where(cd => cd.CustomerId == customerId)
            .Select(cd => cd.LoyaltyPoints)
            .SingleOrDefaultAsync();
    }

    public async Task BuyCoupon(int couponId, int customerId)
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