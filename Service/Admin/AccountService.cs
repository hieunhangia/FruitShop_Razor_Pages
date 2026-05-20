using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Users;
using Service.DTOs;
using Service.DTOs.Admin.Account;

namespace Service.Admin;

public class AccountService(UserManager<User> userManager, AppDbContext context, AccountMapper mapper)
{
    public async Task<int> CountUserAsync() => await userManager.Users.CountAsync();

    public async Task<int> CountSalesStaffAsync() => (await userManager.GetUsersInRoleAsync(Role.SalesStaff)).Count;

    public async Task<int> CountShipperAsync() => (await userManager.GetUsersInRoleAsync(Role.Shipper)).Count;

    public async Task<int> CountCustomerSupportAsync() =>
        (await userManager.GetUsersInRoleAsync(Role.CustomerSupport)).Count;

    public async Task<int> CountCustomerAsync() => (await userManager.GetUsersInRoleAsync(Role.Customer)).Count;

    public async Task<PagedAndSortedDto<AccountDto>> GetAccountsAsync(
        PagedAndSortedRequest<AccountFilter> pagedAndSortedRequest)
    {
        var query = userManager.Users.AsNoTracking();

        var id = pagedAndSortedRequest.Filter.Id?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(id))
        {
            query = query.Where(u => u.Id.ToString().Contains(id));
        }

        var email = pagedAndSortedRequest.Filter.Email?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(u => u.Email!.Contains(email));
        }

        if (pagedAndSortedRequest.Filter.EmailConfirmed.HasValue)
        {
            query = query.Where(u => u.EmailConfirmed == pagedAndSortedRequest.Filter.EmailConfirmed.Value);
        }

        if (pagedAndSortedRequest.Filter.IsLockedOut.HasValue)
        {
            query = pagedAndSortedRequest.Filter.IsLockedOut.Value
                ? query.Where(u => u.LockoutEnd > DateTimeOffset.UtcNow)
                : query.Where(u => u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow);
        }

        pagedAndSortedRequest.SortColumn ??= nameof(User.Id);
        pagedAndSortedRequest.SortDirection ??= SortDirection.Ascending;

        var count = await query.CountAsync();
        if (count == 0)
        {
            return new PagedAndSortedDto<AccountDto>([], 0, pagedAndSortedRequest.PageIndex,
                pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn,
                pagedAndSortedRequest.SortDirection.Value);
        }

        var users = await query
            .DynamicOrderBy(pagedAndSortedRequest.SortColumn, pagedAndSortedRequest.SortDirection.Value)
            .Skip((pagedAndSortedRequest.PageIndex - 1) * pagedAndSortedRequest.PageSize)
            .Take(pagedAndSortedRequest.PageSize)
            .ToListAsync();

        var accounts = new List<AccountDto>();
        foreach (var u in users)
        {
            var account = mapper.ToAccountDto(u);
            account.Roles = (await userManager.GetRolesAsync(u)).ToList();
            accounts.Add(account);
        }

        return new PagedAndSortedDto<AccountDto>(accounts, count, pagedAndSortedRequest.PageIndex,
            pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn,
            pagedAndSortedRequest.SortDirection.Value);
    }

    public async Task<AccountDto> GetAccountDetailAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        var account = mapper.ToAccountDto(user);
        account.Roles = (await userManager.GetRolesAsync(user)).ToList();
        return account;
    }

    public async Task UpdateLockoutEndAsync(int id, DateTimeOffset? lockoutEnd)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.SetLockoutEndDateAsync(user, lockoutEnd);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task RemoveSalesStaffRoleAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.RemoveFromRoleAsync(user, Role.SalesStaff);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task AddSalesStaffRoleAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.AddToRoleAsync(user, Role.SalesStaff);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task RemoveShipperRoleAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.RemoveFromRoleAsync(user, Role.Shipper);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        var shipperData = await context.ShipperData.FirstOrDefaultAsync(sd => sd.ShipperId == user.Id);
        if (shipperData != null)
        {
            context.ShipperData.Remove(shipperData);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddShipperRoleAsync(int id, ShipperDataDto shipperData)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.AddToRoleAsync(user, Role.Shipper);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        user.PhoneNumber = shipperData.PhoneNumber;
        user.ShipperData = new ShipperData { ShipperName = shipperData.Name };
        result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task RemoveCustomerSupportRoleAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.RemoveFromRoleAsync(user, Role.CustomerSupport);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task AddCustomerSupportRoleAsync(int id)
    {
        var user = await GetUserByIdAsync(id);
        var result = await userManager.AddToRoleAsync(user, Role.CustomerSupport);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        user.CustomerSupportData = new CustomerSupportData();
        result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task<ShipperDataDto> GetShipperDataAsync(int id)
    {
        var user = await userManager.Users.AsNoTracking()
            .Include(u => u.ShipperData)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"Người dùng với Id {id} không tồn tại");
        }

        if (await userManager.IsInRoleAsync(user, Role.Shipper) &&
            user is { ShipperData: not null, PhoneNumber: not null })
        {
            return new ShipperDataDto
            {
                Name = user.ShipperData.ShipperName,
                PhoneNumber = user.PhoneNumber
            };
        }

        throw new Exception(
            "Người dùng không có vai trò Người giao hàng hoặc không có dữ liệu của Người giao hàng");
    }

    public async Task UpdateShipperDataAsync(int id, ShipperDataDto shipperData)
    {
        var user = await GetUserByIdAsync(id);
        if (await userManager.IsInRoleAsync(user, Role.Shipper))
        {
            user.ShipperData = new ShipperData { ShipperName = shipperData.Name };
            user.PhoneNumber = shipperData.PhoneNumber;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new Exception(string.Join(", ", errors));
            }
        }
        else
        {
            throw new Exception("Người dùng không có vai trò Người giao hàng");
        }
    }


    private async Task<User> GetUserByIdAsync(int id)
    {
        return await userManager.FindByIdAsync(id.ToString()) ??
               throw new Exception($"Người dùng với Id {id} không tồn tại");
    }
}