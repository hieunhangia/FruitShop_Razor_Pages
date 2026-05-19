using System.ComponentModel.DataAnnotations;
using Repository.Constants;

namespace Repository.Models.Orders;

public class OrderShipping
{
    public int Id { get; set; }

    [Required] public required ShippingStatus ShippingStatus { get; set; }

    [Required] public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}