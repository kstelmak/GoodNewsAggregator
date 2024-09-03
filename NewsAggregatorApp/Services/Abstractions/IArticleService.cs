using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface IArticleService
    {
        Task<ArticleDto?[]> GetArticlesAsync(int pageNumber, int pageSize, CancellationToken token = default);
        //Task<Article?[]> GetTopAsync(int take);
        Task<ArticleDto?> GetArticleByIdAsync(Guid id);
        Task AddArticleAsync(ArticleDto articleDto);
        Task DeleteArticleAsync(/*ArticleDto articleDto*/Guid id);
        Task EditArticleAsync(ArticleDto updatedArticleDto);
        Task<int> GetArticlesCountAsync();
        Task AggregateAsync(CancellationToken token);
        Task SetRateAsync(Guid id, double newRate, CancellationToken token);
        Task LikeAsync(Guid id, string name);
        Task<ArticleWithCommentsModel> GetDetailsAsync(Guid id);
    }
}
