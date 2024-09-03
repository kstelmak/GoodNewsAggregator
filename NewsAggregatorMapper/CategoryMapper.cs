using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMapper
{
    [Mapper]
    public static partial class CategoryMapper
    {
        [MapProperty(nameof(Category.CategoryId), nameof(CategoryDto.CategoryDtoId))]
        public static partial CategoryDto? CategoryToCategoryDto(Category? category);

        [MapProperty(nameof(CategoryDto.CategoryDtoId), nameof(Category.CategoryId))]
        public static partial Category? CategoryDtoToCategory(CategoryDto? categoryDto);
    }
}
