using NewsAggregatorApp.Services.Abstractions;

namespace NewsAggregatorApp.Services
{
    public class ArticleRateService:IArticleRateService
    {
        private readonly IArticleService _articleService;

        public ArticleRateService(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task SetArticleRateAsync(Guid articleId, double newRate, CancellationToken token = default)
        {
            if (newRate is > 5 or < -5)
            {
                throw new ArgumentException("Incorrect rate", nameof(newRate));
            }
            var article = await _articleService.GetArticleByIdAsync(articleId);
            if (article != null)
            {
                await _articleService.SetRateAsync(articleId, newRate, token);
            }
            else
            {
                throw new ArgumentException("Article with that Id doesn't exist", nameof(articleId));
            }
        }
    }
}
