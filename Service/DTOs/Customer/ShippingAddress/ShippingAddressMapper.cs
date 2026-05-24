using Repository.Models.Address;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Customer.ShippingAddress;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class ShippingAddressMapper
{
    [MapProperty(
        $"{nameof(Repository.Models.Users.ShippingAddress.Commune)}.{nameof(Commune.Province)}.{nameof(Province.Name)}",
        nameof(ShippingAddressDto.ProvinceName))]
    [MapProperty(
        $"{nameof(Repository.Models.Users.ShippingAddress.Commune)}.{nameof(Commune.Province)}.{nameof(Province.Code)}",
        nameof(ShippingAddressDto.ProvinceCode))]
    private static partial ShippingAddressDto ToShippingAddressDto(
        Repository.Models.Users.ShippingAddress shippingAddress);

    public static partial IQueryable<ShippingAddressDto> ProjectToShippingAddressDto(
        this IQueryable<Repository.Models.Users.ShippingAddress> shippingAddresses);

    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Id))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.CustomerData))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.CustomerId))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Commune))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.IsDefault))]
    public static partial Repository.Models.Users.ShippingAddress ToShippingAddress(
        AddShippingAddressDto addShippingAddressDto);

    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.CustomerData))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.CustomerId))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.Commune))]
    [MapperIgnoreTarget(nameof(Repository.Models.Users.ShippingAddress.IsDefault))]
    public static partial void UpdateExistingAddress(UpdateShippingAddressDto updateShippingAddressDto,
        Repository.Models.Users.ShippingAddress existingAddress);
}