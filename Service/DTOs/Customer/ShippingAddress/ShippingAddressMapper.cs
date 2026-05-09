using Repository.Models.Address;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.ShippingAddress;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
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

    public partial Repository.Models.Users.ShippingAddress ToShippingAddress(
        AddShippingAddressDto addShippingAddressDto);

    public partial Repository.Models.Users.ShippingAddress ToShippingAddress(
        UpdateShippingAddressDto updateShippingAddressDto);

    public partial void UpdateExistingAddress(UpdateShippingAddressDto updateShippingAddressDto,
        Repository.Models.Users.ShippingAddress existingAddress);
}