using Microsoft.AspNetCore.Identity;

namespace Repository.Models.Users;

public class User : IdentityUser<int>
{
    public CustomerData? CustomerData { get; set; }
    public ShipperData? ShipperData { get; set; }
    public CustomerSupportData? CustomerSupportData { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}