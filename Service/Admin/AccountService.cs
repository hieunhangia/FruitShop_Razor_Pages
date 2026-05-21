using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Constants;
using Repository.Data.Extensions;
using Repository.Models.Users;
using Service.DTOs;
using Service.DTOs.Admin.Account;

namespace Service.Admin;

public class AccountService(AppDbContext context, UserManager<User> userManager, AccountMapper mapper)
{
    public async Task<int> CountUserAsync() => await context.Users.CountAsync();

    public async Task<int> CountSalesStaffAsync()
    {
        var salesStaffRole = await GetRoleByNameAsync(Role.SalesStaff);
        return await context.UserRoles.CountAsync(ur => ur.RoleId == salesStaffRole.Id);
    }

    public async Task<int> CountShipperAsync()
    {
        var shipperRole = await GetRoleByNameAsync(Role.Shipper);
        return await context.UserRoles.CountAsync(ur => ur.RoleId == shipperRole.Id);
    }

    public async Task<int> CountCustomerSupportAsync()
    {
        var customerSupportRole = await GetRoleByNameAsync(Role.CustomerSupport);
        return await context.UserRoles.CountAsync(ur => ur.RoleId == customerSupportRole.Id);
    }

    public async Task<int> CountCustomerAsync()
    {
        var customerRole = await GetRoleByNameAsync(Role.Customer);
        return await context.UserRoles.CountAsync(ur => ur.RoleId == customerRole.Id);
    }

    public async Task<PagedAndSortedDto<AccountDto>> GetAccountsAsync(
        PagedAndSortedRequest<AccountFilter> pagedAndSortedRequest)
    {
        var query = context.Users.AsNoTracking();

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

        var accounts = mapper.ToAccountDtoList(users);
        var userIds = users.Select(u => u.Id).ToList();
        var userRoles = await context.UserRoles
            .Where(ur => userIds.Contains(ur.UserId))
            .Join(context.Roles, ur => ur.RoleId, r => r.Id,
                (ur, r) => new { ur.UserId, RoleName = r.Name! })
            .ToListAsync();

        var rolesLookup = userRoles.GroupBy(x => x.UserId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.RoleName).ToList());

        foreach (var account in accounts)
        {
            account.Roles = rolesLookup.TryGetValue(account.Id, out var roles) ? roles : [];
        }

        return new PagedAndSortedDto<AccountDto>(accounts, count, pagedAndSortedRequest.PageIndex,
            pagedAndSortedRequest.PageSize, pagedAndSortedRequest.SortColumn,
            pagedAndSortedRequest.SortDirection.Value);
    }

    public async Task<AccountDto> GetAccountDetailAsync(int id)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"Người dùng với Id {id} không tồn tại");
        }

        var account = mapper.ToAccountDto(user);
        account.Roles = (await userManager.GetRolesAsync(user)).ToList();
        return account;
    }

    public async Task CreateStaffAccountAsync(CreateStaffAccountDto createStaffAccountDto)
    {
        var roles = await context.Roles
            .Where(r => createStaffAccountDto.SelectedRoleNames.Contains(r.Name!))
            .Select(r => r.Name!)
            .ToListAsync();

        if (roles.Count != createStaffAccountDto.SelectedRoleNames.Count)
        {
            throw new Exception(
                "Một hoặc nhiều vai trò không tồn tại. Vui lòng kiểm tra lại danh sách vai trò đã chọn.");
        }

        if (roles.Contains(Role.Admin) || roles.Contains(Role.Manager))
        {
            throw new Exception("Không thể tạo tài khoản có vai trò Quản trị viên hoặc Quản lý");
        }

        var user = new User
        {
            Email = createStaffAccountDto.Email,
            UserName = createStaffAccountDto.Email,
            EmailConfirmed = true
        };

        if (roles.Contains(Role.Shipper))
        {
            if (createStaffAccountDto.ShipperData != null)
            {
                user.PhoneNumber = createStaffAccountDto.ShipperData.PhoneNumber;
                user.ShipperData = new ShipperData { ShipperName = createStaffAccountDto.ShipperData.Name };
            }
            else
            {
                throw new Exception("Dữ liệu của Người giao hàng là bắt buộc khi vai trò Người giao hàng được chọn");
            }
        }


        var result = await userManager.CreateAsync(user, createStaffAccountDto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        result = await userManager.AddToRolesAsync(user, roles);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }
    }

    public async Task UpdateLockoutEndAsync(int id, DateTimeOffset? lockoutEnd)
    {
        var user = await GetUserByIdAsync(id);
        if (await userManager.IsInRoleAsync(user, Role.Admin) || await userManager.IsInRoleAsync(user, Role.Manager))
        {
            throw new Exception("Không thể cập nhật trạng thái khóa tài khoản của Quản trị viên hoặc Quản lý");
        }

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
        var user = await context.Users
            .Include(u => u.ShipperData)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"Người dùng với Id {id} không tồn tại");
        }

        if (await context.Orders.AnyAsync(o => o.ShipperId == user.Id))
        {
            throw new Exception(
                "Không thể Gỡ quyền Người giao hàng của tài khoản này vì đã có dữ liệu liên quan liên kết với nó.");
        }

        var result = await userManager.RemoveFromRoleAsync(user, Role.Shipper);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        if (user.ShipperData != null)
        {
            context.ShipperData.Remove(user.ShipperData);
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

        result = await userManager.SetPhoneNumberAsync(user, shipperData.PhoneNumber);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        user.ShipperData = new ShipperData { ShipperName = shipperData.Name };
        await context.SaveChangesAsync();
    }

    public async Task RemoveCustomerSupportRoleAsync(int id)
    {
        var user = await context.Users
            .Include(u => u.CustomerSupportData)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"Người dùng với Id {id} không tồn tại");
        }

        if (await context.ProductReviews.AnyAsync(pw => pw.AssignedCustomerSupportId == user.Id))
        {
            throw new Exception(
                "Không thể Gỡ quyền Nhân viên CSKH của tài khoản này vì đã có dữ liệu liên quan liên kết với nó.");
        }

        var result = await userManager.RemoveFromRoleAsync(user, Role.CustomerSupport);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description).ToList();
            throw new Exception(string.Join(", ", errors));
        }

        if (user.CustomerSupportData != null)
        {
            context.CustomerSupportData.Remove(user.CustomerSupportData);
            await context.SaveChangesAsync();
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
        await context.SaveChangesAsync();
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
        var user = await context.Users.Include(u => u.ShipperData).FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new Exception($"Người dùng với Id {id} không tồn tại");
        }

        if (await userManager.IsInRoleAsync(user, Role.Shipper) && user.ShipperData != null)
        {
            var result = await userManager.SetPhoneNumberAsync(user, shipperData.PhoneNumber);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new Exception(string.Join(", ", errors));
            }

            user.ShipperData.ShipperName = shipperData.Name;
            await context.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Người dùng không có vai trò Người giao hàng hoặc không có sẵn dữ liệu giao hàng");
        }
    }


    private async Task<User> GetUserByIdAsync(int id) =>
        await context.Users.FirstOrDefaultAsync(u => u.Id == id) ??
        throw new Exception($"Người dùng với Id {id} không tồn tại");

    private async Task<IdentityRole<int>> GetRoleByNameAsync(string roleName) =>
        await context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == roleName) ??
        throw new Exception($"Vai trò với tên {roleName} không tồn tại");
}