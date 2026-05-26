using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Repository;
using Repository.Constants;
using Repository.Models.Users;
using Service.DTOs.Customer.Account;

namespace Service.Customer;

public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService)
{
    public async Task RegisterAsync(RegisterDto registerDto, Func<int, string, string> createConfirmEmailUrl)
    {
        var user = await userManager.FindByEmailAsync(registerDto.Email);
        if (user != null)
        {
            throw new Exception(
                $"Email '{registerDto.Email}' đã được sử dụng. Nếu bạn là chủ sở hữu tài khoản, vui lòng đăng nhập hoặc sử dụng chức năng quên mật khẩu để khôi phục tài khoản.");
        }

        user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            CustomerData = new CustomerData
            {
                LoyaltyPoints = BusinessRuleConstants.LoyaltyPoint.LoyaltyPointEarnedWhenRegister
            }
        };
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(" ", result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(user, Role.Customer);
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _ = emailService.SendEmailAsync(registerDto.Email, "Xác nhận tài khoản",
            $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: <a href='{createConfirmEmailUrl(user.Id, code)}'>Xác nhận email</a>.<br/>Liên kết này sẽ hết hạn sau {BusinessRuleConstants.Identity.TokenLifespan.EmailConfirmationMinutes} phút.");
    }

    public async Task LoginAsync(LoginDto loginDto)
    {
        var result = await signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            return;
        }

        if (result.IsLockedOut)
        {
            throw new Exception("Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau.");
        }

        if (result.IsNotAllowed)
        {
            throw new Exception(
                "Vui lòng xác nhận email của bạn để đăng nhập. Nếu email xác nhận đã hết hạn, vui lòng chọn 'Gửi lại email xác nhận' để nhận email mới.");
        }

        throw new Exception("Đăng nhập thất bại. Vui lòng kiểm tra email và mật khẩu của bạn.");
    }

    public async Task<bool> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        var user = await userManager.FindByIdAsync(confirmEmailDto.UserId);
        if (user == null)
        {
            return false;
        }

        confirmEmailDto.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmEmailDto.Code));
        var result = await userManager.ConfirmEmailAsync(user, confirmEmailDto.Code);
        return result.Succeeded;
    }

    public async Task ResendConfirmEmailAsync(ResendConfirmEmailDto resendConfirmEmailDto,
        Func<int, string, string> createConfirmEmailUrl)
    {
        var user = await userManager.FindByEmailAsync(resendConfirmEmailDto.Email);
        if (user is { EmailConfirmed: false })
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _ = emailService.SendEmailAsync(resendConfirmEmailDto.Email, "Xác nhận tài khoản",
                $"Vui lòng xác nhận email của bạn bằng cách nhấp vào liên kết sau: <a href='{createConfirmEmailUrl(user.Id, code)}'>Xác nhận email</a>.<br/>Liên kết này sẽ hết hạn sau {BusinessRuleConstants.Identity.TokenLifespan.EmailConfirmationMinutes} phút.");
        }
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto,
        Func<int, string, string> createResetPasswordUrl)
    {
        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
        if (user != null)
        {
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _ = emailService.SendEmailAsync(forgotPasswordDto.Email, "Đặt lại mật khẩu",
                $"Vui lòng đặt lại mật khẩu của bạn bằng cách nhấp vào liên kết sau: <a href='{createResetPasswordUrl(user.Id, code)}'>Đặt lại mật khẩu</a>.<br/>Liên kết này sẽ hết hạn sau {BusinessRuleConstants.Identity.TokenLifespan.PasswordResetMinutes} phút.");
        }
    }

    public async Task<bool> VerifyResetPasswordRequestAsync(VerifyResetPasswordRequestDto verifyResetPasswordRequestDto)
    {
        var user = await userManager.FindByIdAsync(verifyResetPasswordRequestDto.UserId);
        if (user == null)
        {
            return false;
        }

        try
        {
            return await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider,
                UserManager<User>.ResetPasswordTokenPurpose,
                Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(verifyResetPasswordRequestDto.Code)));
        }
        catch
        {
            return false;
        }
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await userManager.FindByIdAsync(resetPasswordDto.UserId);
        if (user == null)
        {
            throw new Exception("Đặt lại mật khẩu thất bại. Vui lòng thử lại.");
        }

        var result = await userManager.ResetPasswordAsync(user,
            Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.Code)), resetPasswordDto.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(" ", result.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsEmailConfirmedAsync(user))
        {
            await userManager.ConfirmEmailAsync(user, await userManager.GenerateEmailConfirmationTokenAsync(user));
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var user = await userManager.FindByIdAsync(changePasswordDto.UserId);
        if (user == null)
        {
            throw new Exception("Đã có lỗi xảy ra trong quá trình đổi mật khẩu. Vui lòng thử lại.");
        }

        var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
            changePasswordDto.NewPassword);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(" ", result.Errors.Select(e => e.Description)));
        }
    }
}