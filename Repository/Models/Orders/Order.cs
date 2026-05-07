using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Constants;
using Repository.Models.Users;

namespace Repository.Models.Orders;

public class Order
{
    public int Id { get; set; }

    [Required] public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required] public required OrderStatus Status { get; set; }

    [Required] public required PaymentMethod PaymentMethod { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public required string ShippingAddressSnapshot { get; set; }

    [Required] public required int CustomerId { get; set; }
    public User? Customer { get; set; }

    public int? ShipperId { get; set; }
    public User? Shipper { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }

    public int? QrCodePaymentDataId { get; set; }
    public OrderQrCodePaymentData? QrCodePaymentData { get; set; }

    public ICollection<OrderShipping>? OrderShippings { get; set; }
}