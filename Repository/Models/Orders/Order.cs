using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Repository.Constants;
using Repository.Models.Users;

namespace Repository.Models.Orders;

public class Order
{
    public int Id { get; set; }

    [Required] public required DateTime OrderDate { get; set; }

    [Required] public required OrderStatus Status { get; set; }

    [Required] public required PaymentMethod PaymentMethod { get; set; }

    [Required]
    [Column(TypeName = "jsonb")]
    public required string ShippingAddressSnapshot { get; set; }

    [Required] public int CustomerId { get; set; }
    public User? Customer { get; set; }

    public int? ShipperId { get; set; }
    public User? Shipper { get; set; }

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

    public static ShippingAddressSnapshot FromShippingAddress(ShippingAddress shippingAddress) => new()
    {
        RecipientName = shippingAddress.RecipientName,
        RecipientPhoneNumber = shippingAddress.RecipientPhoneNumber,
        SpecificAddress = shippingAddress.SpecificAddress,
        CommuneName = shippingAddress.Commune?.Name ?? string.Empty,
        ProvinceName = shippingAddress.Commune?.Province?.Name ?? string.Empty
    };

    public static ShippingAddressSnapshot? FromJson(string json) =>
        JsonSerializer.Deserialize<ShippingAddressSnapshot>(json);

    public string ToJson() => JsonSerializer.Serialize(this);
}