using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Address;

namespace Service;

public class AddressService(AppDbContext context, AddressMapper mapper)
{
    public async Task<List<ProvinceDto>> GetProvincesAsync() =>
        mapper.ToProvinceDtoList(await context.Provinces.AsNoTracking().ToListAsync());

    public async Task<ProvinceDto?> GetProvinceByCodeAsync(string provinceCode)
    {
        var province = await context.Provinces.AsNoTracking().FirstOrDefaultAsync(p => p.Code == provinceCode);
        return province == null ? null : mapper.ToProvinceDto(province);
    }

    public async Task<List<CommuneDto>> GetCommunesByProvinceCodeAsync(string provinceCode) =>
        mapper.ToCommuneDtoList(await context.Communes.AsNoTracking().Where(c => c.ProvinceCode == provinceCode)
            .ToListAsync());

    public async Task<CommuneDto?> GetCommuneByCodeAsync(string communeCode)
    {
        var commune = await context.Communes.AsNoTracking().FirstOrDefaultAsync(c => c.Code == communeCode);
        return commune == null ? null : mapper.ToCommuneDto(commune);
    }
}