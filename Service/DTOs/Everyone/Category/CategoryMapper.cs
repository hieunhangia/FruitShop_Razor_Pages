using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Category;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CategoryMapper
{
    public partial CategoryDto ToCategoryDto(Repository.Models.Products.Category category);
    public partial List<CategoryDto> ToCategoryDtoList(List<Repository.Models.Products.Category> categories);
}