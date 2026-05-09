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
}