using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Constants;
using Repository.Models.Coupons;
using Repository.Models.Users;

namespace Repository.Models.Orders;

public class Order
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public required long Id { get; set; }

    [Required] public required DateTime OrderDate { get; set; }

    [Required] public required OrderStatus OrderStatus { get; set; }

    [Required] public required PaymentMethod PaymentMethod { get; set; }

    [Required] public required long TotalAmountBeforeDiscount { get; set; }

    [Required] public required long TotalAmount { get; set; }

    [Required] public required ShippingAddressSnapshot ShippingAddressSnapshot { get; set; }

    public int? CustomerCouponId { get; set; }
    public CustomerCoupon? CustomerCoupon { get; set; }

    [Required] public int CustomerId { get; set; }
    public CustomerData? Customer { get; set; }

    public int? ShipperId { get; set; }
    public ShipperData? Shipper { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }

    public int? QrCodePaymentDataId { get; set; }
    public OrderQrCodePaymentData? QrCodePaymentData { get; set; }

    public ICollection<OrderShipping>? OrderShippings { get; set; }
}

public class ShippingAddressSnapshot
{
    public required string RecipientName { get; set; }
    public required string RecipientPhoneNumber { get; set; }
    public required string SpecificAddress { get; set; }
    public required string CommuneName { get; set; }
    public required string ProvinceName { get; set; }
}