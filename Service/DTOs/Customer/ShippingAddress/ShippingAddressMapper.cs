using Repository.Models.Address;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.ShippingAddress;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ShippingAddressMapper
{
    [MapProperty(
        $"{nameof(Repository.Models.Users.ShippingAddress.Commune)}.{nameof(Commune.Province)}.{nameof(Province.Name)}",
        nameof(ShippingAddressDto.ProvinceName))]
    [MapProperty(
        $"{nameof(Repository.Models.Users.ShippingAddress.Commune)}.{nameof(Commune.Province)}.{nameof(Province.Code)}",
        nameof(ShippingAddressDto.ProvinceCode))]
    public partial ShippingAddressDto ToShippingAddressDto(Repository.Models.Users.ShippingAddress shippingAddress);

    public partial List<ShippingAddressDto> ToShippingAddressDtoList(
        List<Repository.Models.Users.ShippingAddress> shippingAddresses);

    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Id))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Customer))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.CustomerId))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Commune))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.IsDefault))]
    public partial Repository.Models.Users.ShippingAddress ToShippingAddress(
        AddShippingAddressDto addShippingAddressDto);

    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Customer))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.CustomerId))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Commune))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.IsDefault))]
    public partial void UpdateExistingAddress(UpdateShippingAddressDto updateShippingAddressDto,
        Repository.Models.Users.ShippingAddress existingAddress);
}