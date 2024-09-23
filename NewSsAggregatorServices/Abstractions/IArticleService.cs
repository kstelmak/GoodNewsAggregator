using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Queries.Articles;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApp.Services.Abstractions
{
	public interface IArticleService
	{
		Task<ArticleDto?[]> GetArticlesAsync(int pageNumber, int pageSize, int minRate, CancellationToken token);
		Task<ArticleWithCommentsModel> GetDetailsAsync(Guid id, CancellationToken token);
		Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token);
		//Task<Article?[]> GetTopAsync(int take);
		
		Task<int> GetArticlesCountAsync(int minRate, CancellationToken token);


        Task AddArticleAsync(ArticleDto articleDto, CancellationToken token);
		Task DeleteArticleAsync(/*ArticleDto articleDto*/Guid id, CancellationToken token);
		Task EditArticleAsync(ArticleDto updatedArticleDto, CancellationToken token);
		Task AggregateAsync(CancellationToken token);
		Task AggregateInBackgroundAsync(CancellationToken token);
		Task SetRateAsync(Guid id, double newRate, CancellationToken token);
		Task GetRssDataFromSourcesAsync(CancellationToken token);
		//Task<ArticleDto[]> GetArticlesWithoutTextAsync(CancellationToken token);
		//Task UpdateTextAsync(ArticleDto articleDto, CancellationToken token);
		Task GetArticlesWithoutTextAndUpdateTextAsync(CancellationToken token);


	}
}
