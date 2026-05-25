using Riok.Mapperly.Abstractions;

namespace Service.DTOs.Everyone.Category;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CategoryMapper
{
    public static partial CategoryDto ToCategoryDto(this Repository.Models.Products.Category category);

    public static partial IQueryable<CategoryDto> ProjectToCategoryDto(
        this IQueryable<Repository.Models.Products.Category> categories);
}