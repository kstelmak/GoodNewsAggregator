namespace NewsAggregatorApp.Services.Abstractions
{
    public interface IArticleRateService
    {
        Task SetArticleRateAsync(Guid articleId, double newRate, CancellationToken token = default);

    }
}
