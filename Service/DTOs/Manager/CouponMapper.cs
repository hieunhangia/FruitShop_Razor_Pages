using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Manager;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CouponMapper
{
        public partial CouponUpdateDto ToCouponUpdateDto(Repository.Models.Coupons.Coupon coupon);
        
}