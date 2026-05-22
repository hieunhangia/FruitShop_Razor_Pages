namespace Repository;

public static class BusinessRuleConstants
{
    public static class Model
    {
        public static class Commune
        {
            public const int CodeMaxLength = 10;
            public const int NameMaxLength = 50;
        }

        public static class Province
        {
            public const int CodeMaxLength = 10;
            public const int NameMaxLength = 50;
        }

        public static class ShippingAddress
        {
            public const int RecipientNameMaxLength = 100;
            public const int RecipientPhoneNumberLength = 10;
            public const string RecipientPhoneNumberPattern = @"^0\d{9}$";
            public const int SpecificAddressMaxLength = 255;
        }

        public static class ShipperData
        {
            public const int NameMaxLength = 100;
            public const string PhoneNumberPattern = @"^0\d{9}$";
        }

        public static class Product
        {
            public const int NameMaxLength = 50;
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
        }

        public static class ProductUnit
        {
            public const int NameMaxLength = 50;
        }

        public static class ProductReview
        {
            public const int RatingMinValue = 1;
            public const int RatingMaxValue = 5;
            public const int CommentMaxLength = 1000;
            public const int ResolutionMessageMaxLength = 1000;
        }

        public static class Coupon
        {
            public const int DescriptionMaxLength = 1000;
        }
    }

    public static class Identity
    {
        public static class Password
        {
            public const int RequiredLength = 8;
            public const int MaxLength = 100;
            public const bool RequireDigit = true;
            public const bool RequireLowercase = true;
            public const bool RequireUppercase = true;
            public const bool RequireNonAlphanumeric = false;
            public const int RequiredUniqueChars = 1;
        }

        public static class Lockout
        {
            public const int LockoutMinutes = 5;
            public const int MaxFailedAccessAttempts = 5;
        }

        public static class TokenLifespan
        {
            public const int EmailConfirmationMinutes = 60;
            public const int PasswordResetMinutes = 60;
        }
    }

    public static class FileService
    {
        public const string PathRegexPattern = @"^[a-zA-Z0-9\-_ ]+(/[a-zA-Z0-9\-_ ]+)*/$";
        public const string ProductImagesPath = "images/products/";
        public const int PrivateFileUrlExpirationSeconds = 900;
    }

    public static class Order
    {
        public static long GenerateUniqueOrderId() => Random.Shared.NextInt64(1000, 10000);
        public const int QrCodePaymentOrderExpiredMinutes = 5;
        public const int CancelExpiredQrCodePaymentOrderBackgroundServiceDelayMinutes = 5;
        public const int MinProductQuantity = 1;
    }

    public static class Coupon
    {
        public const long MaxDiscountValue = long.MaxValue;
        public const long MaxLoyaltyPointsCost = long.MaxValue;
        public const long MaxMinOrderAmount = long.MaxValue;
        public static DateTime ExpiryDateTime => DateTime.UtcNow.AddDays(30);
    }

    public static class LoyaltyPoint
    {
        public static int CalculateLoyaltyPoints(long totalAmount) => (int)Math.Max(0, totalAmount / 1000);
    }

    public static class HealthCheck
    {
        public const string HealthCheckApi = "/api/admin/health-check";
        public const string PayOsApiHealthCheck = "https://api-merchant.payos.vn/";
        public const string MinioApiHealthCheck = "http://localhost:9000/minio/health/live";
        public const int MinimumFreeSpaceUnhealthyGB = 1;
        public const int MaximumRamUsageUnhealthyMB = 1024;
    }

    public static class AdminPageRoute
    {
        public const string ErrorLogPage = "/admin/error-logs";
    }

    public static class CouponPageValue
    {
        public const int DefaultIndex = 1;
        public const int NumberOfElement = 6;
    }
}