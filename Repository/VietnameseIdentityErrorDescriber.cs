using Microsoft.AspNetCore.Identity;

namespace Repository;

public class VietnameseIdentityErrorDescriber: IdentityErrorDescriber
{
    public override IdentityError DefaultError() =>
        new()
        {
            Code = nameof(DefaultError),
            Description = "Đã xảy ra lỗi khi thực hiện yêu cầu."
        };

    public override IdentityError PasswordMismatch() =>
        new()
        {
            Code = nameof(PasswordMismatch),
            Description = "Mật khẩu không đúng."
        };

    public override IdentityError InvalidToken() =>
        new()
        {
            Code = nameof(InvalidToken),
            Description = "Token cho yêu cầu này không hợp lệ."
        };

    public override IdentityError InvalidUserName(string? userName) =>
        new()
        {
            Code = nameof(InvalidUserName),
            Description = $"Tên người dùng '{userName}' không hợp lệ."
        };

    public override IdentityError InvalidEmail(string? email) =>
        new()
        {
            Code = nameof(InvalidEmail),
            Description = $"Email '{email}' không hợp lệ."
        };

    public override IdentityError DuplicateUserName(string userName) =>
        new()
        {
            Code = nameof(DuplicateUserName),
            Description = $"Tên người dùng '{userName}' đã được sử dụng."
        };

    public override IdentityError DuplicateEmail(string email) =>
        new()
        {
            Code = nameof(DuplicateEmail),
            Description = $"Email '{email}' đã được sử dụng."
        };

    public override IdentityError InvalidRoleName(string? role) =>
        new()
        {
            Code = nameof(InvalidRoleName),
            Description = $"Tên vai trò '{role}' không hợp lệ."
        };

    public override IdentityError DuplicateRoleName(string role) =>
        new()
        {
            Code = nameof(DuplicateRoleName),
            Description = $"Tên vai trò '{role}' đã được sử dụng."
        };

    public override IdentityError UserAlreadyInRole(string role) =>
        new()
        {
            Code = nameof(UserAlreadyInRole),
            Description = $"Người dùng đã có vai trò '{role}'."
        };

    public override IdentityError UserNotInRole(string role) =>
        new()
        {
            Code = nameof(UserNotInRole),
            Description = $"Người dùng không có vai trò '{role}'."
        };

    public override IdentityError PasswordTooShort(int length) =>
        new()
        {
            Code = nameof(PasswordTooShort),
            Description = $"Mật khẩu phải có ít nhất {length} ký tự."
        };

    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) =>
        new()
        {
            Code = nameof(PasswordRequiresUniqueChars),
            Description = $"Mật khẩu phải chứa ít nhất {uniqueChars} ký tự duy nhất."
        };

    public override IdentityError PasswordRequiresNonAlphanumeric() =>
        new()
        {
            Code = nameof(PasswordRequiresNonAlphanumeric),
            Description = "Mật khẩu phải chứa ít nhất một ký tự không phải chữ hoặc số."
        };

    public override IdentityError PasswordRequiresDigit() =>
        new()
        {
            Code = nameof(PasswordRequiresDigit),
            Description = "Mật khẩu phải chứa ít nhất một chữ số."
        };

    public override IdentityError PasswordRequiresLower() =>
        new()
        {
            Code = nameof(PasswordRequiresLower),
            Description = "Mật khẩu phải chứa ít nhất một chữ cái viết thường."
        };

    public override IdentityError PasswordRequiresUpper() =>
        new()
        {
            Code = nameof(PasswordRequiresUpper),
            Description = "Mật khẩu phải chứa ít nhất một chữ cái viết hoa."
        };
}