using System.Security.Claims;

namespace FruitShop_Razor_Pages.Extensions;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        public int GetUserId()
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("Đã xảy ra lỗi khi lấy thông tin người dùng. Vui lòng đăng nhập lại.");
            }

            return int.TryParse(userIdClaim.Value, out var userId)
                ? userId
                : throw new Exception("Đã xảy ra lỗi khi lấy thông tin người dùng. Vui lòng đăng nhập lại.");
        }

        public string GetUserEmail()
        {
            var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            return emailClaim == null
                ? throw new Exception("Đã xảy ra lỗi khi lấy thông tin người dùng. Vui lòng đăng nhập lại.")
                : emailClaim.Value;
        }
    }
}