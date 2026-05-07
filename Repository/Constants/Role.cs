namespace Repository.Constants;

public static class Role
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string SalesStaff = "SalesStaff";
    public const string Shipper = "Shipper";
    public const string Customer = "Customer";

    public static readonly string[] AllRoles = [Admin, Manager, SalesStaff, Shipper, Customer];
}