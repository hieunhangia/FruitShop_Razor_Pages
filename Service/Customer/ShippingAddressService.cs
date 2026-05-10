using Microsoft.EntityFrameworkCore;
using Repository;
using Service.DTOs.Customer.ShippingAddress;

namespace Service.Customer;

public class ShippingAddressService(AppDbContext context, ShippingAddressMapper mapper)
{
    public async Task<List<ShippingAddressDto>> GetShippingAddressesByCustomerIdAsync(int customerId) =>
        mapper.ToShippingAddressDtoList(await context.ShippingAddresses.AsNoTracking()
            .Include(sa => sa.Commune)
            .ThenInclude(c => c!.Province)
            .Where(sa => sa.CustomerId == customerId)
            .OrderByDescending(sa => sa.IsDefault)
            .ToListAsync());

    public async Task<ShippingAddressDto?>
        GetShippingAddressByIdAndCustomerIdAsync(int customerId, int shippingAddressId)
    {
        var shippingAddress = await context.ShippingAddresses.AsNoTracking()
            .Include(sa => sa.Commune)
            .ThenInclude(c => c!.Province)
            .FirstOrDefaultAsync(sa => sa.CustomerId == customerId && sa.Id == shippingAddressId);
        return shippingAddress == null ? null : mapper.ToShippingAddressDto(shippingAddress);
    }

    public async Task AddShippingAddressAsync(int customerId, AddShippingAddressDto addShippingAddressDto)
    {
        var shippingAddress = mapper.ToShippingAddress(addShippingAddressDto);
        shippingAddress.CustomerId = customerId;
        if (!await context.ShippingAddresses.AnyAsync(sa => sa.CustomerId == customerId))
        {
            shippingAddress.IsDefault = true;
        }

        context.ShippingAddresses.Add(shippingAddress);
        await context.SaveChangesAsync();
    }

    public async Task UpdateShippingAddressAsync(int customerId, UpdateShippingAddressDto updateShippingAddressDto)
    {
        var existingAddress = await context.ShippingAddresses.FirstOrDefaultAsync(sa =>
            sa.Id == updateShippingAddressDto.Id && sa.CustomerId == customerId);

        if (existingAddress == null)
        {
            throw new Exception("Địa chỉ giao hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        mapper.UpdateExistingAddress(updateShippingAddressDto, existingAddress);
        await context.SaveChangesAsync();
    }

    public async Task SetDefaultShippingAddressAsync(int customerId, int shippingAddressId)
    {
        var shippingAddress = await context.ShippingAddresses
            .FirstOrDefaultAsync(sa => sa.Id == shippingAddressId && sa.CustomerId == customerId);
        if (shippingAddress == null)
        {
            throw new Exception("Địa chỉ giao hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        var currentDefaultShippingAddress = await context.ShippingAddresses
            .FirstOrDefaultAsync(sa => sa.CustomerId == customerId && sa.IsDefault);
        currentDefaultShippingAddress?.IsDefault = false;
        shippingAddress.IsDefault = true;
        await context.SaveChangesAsync();
    }

    public async Task DeleteShippingAddressAsync(int customerId, int shippingAddressId)
    {
        var shippingAddress = await context.ShippingAddresses
            .FirstOrDefaultAsync(sa => sa.Id == shippingAddressId && sa.CustomerId == customerId);
        if (shippingAddress == null)
        {
            throw new Exception("Địa chỉ giao hàng không tồn tại hoặc không thuộc về khách hàng.");
        }

        if (shippingAddress.IsDefault)
        {
            throw new Exception(
                "Không thể xóa địa chỉ giao hàng mặc định. Vui lòng đặt một địa chỉ khác làm mặc định trước khi xóa.");
        }

        context.ShippingAddresses.Remove(shippingAddress);
        await context.SaveChangesAsync();
    }
}