using System.ComponentModel.DataAnnotations;

namespace Repository.Models.Orders;

public class OrderQrCodePaymentData
{
    public int Id { get; set; }

    [Required] public required string PaymentLink { get; set; }

    [Required] public required DateTime ExpirationDate { get; set; }

    public DateTime? PaymentDate { get; set; }
}