namespace Service.DTOs.Everyone.Product;

public class ProductFilter
{
    public string? SearchTerm { get; set; }
    public long? MinPrice { get; set; }
    public long? MaxPrice { get; set; }
    public int? CategoryId { get; set; }
    public int? AverageRating { get; set; }
}