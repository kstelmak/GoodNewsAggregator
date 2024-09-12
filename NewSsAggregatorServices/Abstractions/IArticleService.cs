using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApp.Services.Abstractions
{
    public interface IArticleService
    {
        Task<ArticleDto?[]> GetArticlesAsync(int? pageNumber, int? pageSize, int? minRate, CancellationToken token);
        Task<ArticleWithCommentsModel> GetDetailsAsync(Guid id, CancellationToken token);
        Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token);
        //Task<Article?[]> GetTopAsync(int take);
        //Task<int> GetArticlesCountAsync();

        Task AddArticleAsync(ArticleDto articleDto, CancellationToken token);
        Task DeleteArticleAsync(/*ArticleDto articleDto*/Guid id, CancellationToken token);
        Task EditArticleAsync(ArticleDto updatedArticleDto, CancellationToken token);        
        Task AggregateAsync(CancellationToken token);
        Task SetRateAsync(Guid id, double newRate, CancellationToken token);     
    }
}
