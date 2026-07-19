namespace Service.DTOs.Shipper;

public class ShipperDashboardDto
{
    public ShipperKpiDto KPIs { get; set; } = new();
    public List<DailyDeliveryDto> DeliveryChart { get; set; } = []; // 7 ngày gần nhất
}

public class ShipperKpiDto
{
    public int TotalDeliveredToday { get; set; }
    public int TotalDeliveredThisMonth { get; set; }
    public int ActiveOrders { get; set; }
    public long TotalRevenueDeliveredThisMonth { get; set; }
}

public class DailyDeliveryDto
{
    public string Label { get; set; } = string.Empty; // "dd/MM"
    public int DeliveredCount { get; set; }
}
