namespace Repository.Constants;

public enum OrderStatus
{
    PendingConfirmation,
    PendingPayment,
    Processing,
    Shipping,
    Delivered,
    Cancelled
}