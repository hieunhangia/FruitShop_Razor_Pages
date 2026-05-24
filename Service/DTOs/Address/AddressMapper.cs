using Repository.Models.Address;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Address;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class AddressMapper
{
    public static partial IQueryable<ProvinceDto> ProjectToProvinceDto(this IQueryable<Province> province);

    public static partial IQueryable<CommuneDto> ProjectToCommuneDto(this IQueryable<Commune> commune);
}