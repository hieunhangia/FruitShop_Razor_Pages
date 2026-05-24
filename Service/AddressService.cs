using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Address;

namespace Service;

public class AddressService(AppDbContext context)
{
    public async Task<List<ProvinceDto>> GetProvincesAsync() =>
        await context.Provinces.ProjectToProvinceDto().ToListAsync();

    public async Task<ProvinceDto?> GetProvinceByCodeAsync(string provinceCode) =>
        await context.Provinces
            .Where(p => p.Code == provinceCode)
            .ProjectToProvinceDto()
            .FirstOrDefaultAsync();

    public async Task<List<CommuneDto>> GetCommunesByProvinceCodeAsync(string provinceCode) =>
        await context.Communes
            .Where(c => c.ProvinceCode == provinceCode)
            .ProjectToCommuneDto()
            .ToListAsync();

    public async Task<CommuneDto?> GetCommuneByCodeAsync(string communeCode) =>
        await context.Communes
            .Where(c => c.Code == communeCode)
            .ProjectToCommuneDto()
            .FirstOrDefaultAsync();
}