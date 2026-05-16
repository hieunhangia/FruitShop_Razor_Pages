using Repository.Models.Address;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Address;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AddressMapper
{
    public partial ProvinceDto ToProvinceDto(Province province);

    public partial List<ProvinceDto> ToProvinceDtoList(List<Province> provinces);

    public partial CommuneDto ToCommuneDto(Commune commune);

    public partial List<CommuneDto> ToCommuneDtoList(List<Commune> communes);
}