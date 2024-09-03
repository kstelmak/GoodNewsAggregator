using NewsAggregatorApp.Entities;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface ICategoryService
    {
        Task<Dictionary<Guid, string>?> GetCategoriesIdsAndNamesAsync();
        //Task<string?[]> GetCategoriesNamesAsync();
        //Task<Category[]> GetCategoriesByArticleId(Guid id);
    }
}
