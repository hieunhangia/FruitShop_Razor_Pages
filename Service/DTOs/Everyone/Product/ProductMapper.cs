using Repository.Models.Orders;
using Repository.Models.Products;
using Repository.Models.Users;
using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Product;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProductMapper(FileService fileService)
{
    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductSummaryDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductSummaryDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    public partial ProductSummaryDto ToProductSummaryDto(Repository.Models.Products.Product product);

    public partial List<ProductSummaryDto> ToProductSummaryDtoList(List<Repository.Models.Products.Product> products);

    [MapProperty($"{nameof(ProductReview.Customer)}.{nameof(CustomerData.Customer)}.{nameof(User.Email)}",
        nameof(ProductReviewDto.ReviewerEmail), Use = nameof(MapReviewerEmail))]
    private partial ProductReviewDto ToProductReviewDto(ProductReview productReview);

    [UserMapping(Default = false)]
    private static string MapReviewerEmail(string email)
    {
        if (!email.Contains('@'))
        {
            return email;
        }

        var parts = email.Split('@');
        var username = parts[0];
        var maskedUsername = username.Length > 3 ? username[..3] : username[..1];
        return $"{maskedUsername}***@{parts[1]}";
    }

    [MapProperty($"{nameof(Repository.Models.Products.Product.ProductUnit)}.{nameof(ProductUnit.Name)}",
        nameof(ProductDetailDto.ProductUnitName))]
    [MapProperty(nameof(Repository.Models.Products.Product.ImageFilePath), nameof(ProductDetailDto.ImageUrl),
        Use = nameof(MapImageFilePath))]
    public partial ProductDetailDto ToProductDetailDto(Repository.Models.Products.Product product);


    [UserMapping(Default = false)]
    private string MapImageFilePath(string imageFilePath) => fileService.GetPublicFileUrl(imageFilePath);
}