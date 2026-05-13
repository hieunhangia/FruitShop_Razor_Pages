using Microsoft.AspNetCore.Identity;

namespace Repository.Models.Users;

public class User : IdentityUser<int>
{
    public ShipperInformation? ShipperInformation { get; set; }
}