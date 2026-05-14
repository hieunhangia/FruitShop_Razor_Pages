namespace Service.DTOs.Customer.Order;

public class CreateQrCodePaymentDto
{
    public required string CustomerEmail { get; set; }
    public required int ShippingAddressId { get; set; }
    public required int? CustomerCouponId { get; set; }
    public required string ReturnUrl { get; set; }
    public required string CancelUrl { get; set; }
}