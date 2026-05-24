using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Category;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CategoryMapper
{
    public static partial IQueryable<CategoryDto> ProjectToCategoryDto(
        this IQueryable<Repository.Models.Products.Category> categories);
}