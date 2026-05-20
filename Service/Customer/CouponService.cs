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

    public async Task<PagedAndSortedDto<CouponShopDto>> GetAllAvailableCouponsForSaleAsync(PagedAndSortedRequest<CouponFilter> request, long customerId)
    {
        request.SortColumn ??= nameof(Repository.Models.Coupons.Coupon.Id);
        request.SortDirection ??= SortDirection.Ascending;

        var query = context.Coupons.AsNoTracking()
            .Where(c => c.IsActive);

        var filter = request.Filter;
        if (!string.IsNullOrWhiteSpace(filter.Keyword))
        {
            var keyword = filter.Keyword.Trim().ToLower();
            query = query.Where(c => c.Description.ToLower().Contains(keyword));
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
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var dtos = mapper.ToCouponShopDtoList(items);

        var itemIds = items.Select(c => c.Id).ToList();
        var ownedCouponIds = await context.CustomerCoupons.AsNoTracking()
            .Where(cc =>
                cc.CustomerId == customerId &&
                !cc.IsUsed &&
                (!cc.ExpiryDate.HasValue || cc.ExpiryDate.Value > DateTime.UtcNow) &&
                itemIds.Contains(cc.CouponId))
            .Select(cc => cc.CouponId)
            .ToListAsync();

        if (ownedCouponIds.Count > 0)
        {
            var ownedCouponIdSet = ownedCouponIds.ToHashSet();
            foreach (var dto in dtos)
            {
                dto.CanBuy = !ownedCouponIdSet.Contains(dto.Id);
            }
        }
        
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

        var hasActiveCustomerCoupon = await context.CustomerCoupons.AsNoTracking()
            .AnyAsync(cc =>
                cc.CustomerId == customerId &&
                cc.CouponId == couponId &&
                !cc.IsUsed &&
                cc.ExpiryDate.HasValue &&
                cc.ExpiryDate.Value > DateTime.UtcNow);
        if (hasActiveCustomerCoupon)
        {
            throw new Exception("Bạn đã sở hữu coupon này và vẫn còn hiệu lực.");
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

        var customerCoupon = new Repository.Models.Coupons.CustomerCoupon
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