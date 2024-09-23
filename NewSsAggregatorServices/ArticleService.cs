using Hangfire;
using HtmlAgilityPack;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Mappers;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Commands.Articles;
using NewsAggregatorCQS.Commands.Categories;
using NewsAggregatorCQS.Queries.Articles;
using NewsAggregatorCQS.Queries.Categories;
using NewsAggregatorCQS.Queries.Comments;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Xml;

namespace NewsAggregatorApp.Services
{

    //I think that in this service it makes sense to test only the method AggregateAsync
    //but it calls methods GetRssDataAsync & UpdateTextAsync, which access web pages
    //How to write tests for this method if it accesses a web pages?

    public class ArticleService : IArticleService
    {
        private readonly IArticleRateService _articleRateService;
        private readonly ISourceService _sourceService;
        private readonly IMediator _mediator;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(ILogger<ArticleService> logger, IArticleRateService articleRateService, ISourceService sourceService, IMediator mediator)
        {
            _logger = logger;
            _articleRateService = articleRateService;
            _sourceService = sourceService;
            _mediator = mediator;
        }

        public async Task<ArticleDto?[]> GetArticlesAsync(int pageNumber, int pageSize, int minRate, CancellationToken token)
        {
            try
            {
                return await _mediator.Send(new GetArticlesByPageNumberAndPageSizeQuery()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    MinRate = minRate
                }, token);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<int> GetArticlesCountAsync(int minRate, CancellationToken token)
        {
            try
            {
                return (await _mediator.Send(new GetArticlesQuery(), token))
                    .Where(a => a.Rate >= minRate)
                    .Count();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //public async Task<Article?[]> GetTopAsync(int take)
        //{
        //    return await _context.Articles
        //        .OrderByDescending(article => article!.Rate)
        //        .Take(take)
        //        .ToArrayAsync();
        //}//?????????

        public async Task<ArticleDto?> GetArticleByIdAsync(Guid id, CancellationToken token)
        {
            return await _mediator.Send(new GetArticleByIdQuery() { ArticleId = id }, token);
        }

        public async Task AddArticleAsync(ArticleDto articleDto, CancellationToken token)
        {
            await _mediator.Send(new AddArticleCommand()
            {
                ArticleDto = articleDto
            }, token);
        }

        public async Task DeleteArticleAsync(Guid id, CancellationToken token)
        {

            await _mediator.Send(new DeleteArticleCommand()
            {
                ArticleId = id
            }, token);
        }

        public async Task EditArticleAsync(ArticleDto updatedArticleDto, CancellationToken token)
        {
            await _mediator.Send(new EditArticleCommand() { ArticleDto = updatedArticleDto }, token);
        }

        public async Task<int> GetArticlesCountAsync(CancellationToken token)
        {
            return (await _mediator.Send(new GetArticlesQuery(), token)).Length;
        }

        public async Task AggregateAsync(CancellationToken token)
        {
            try
            {
                await GetRssDataFromSourcesAsync(token);
                await GetArticlesWithoutTextAndUpdateTextAsync(token);
                await RateArticlesWithoutRate(token);

                //RecurringJob.Trigger("GetRssDataFromSources");
                //RecurringJob.Trigger("GetArticlesWithoutTextAndUpdateText");
                //RecurringJob.Trigger("RateArticlesWithoutRate");

                _logger.LogInformation($"Aggregation complited");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task AggregateInBackgroundAsync(CancellationToken token)
        {
            RecurringJob.AddOrUpdate("GetRssDataFromSources",
                () => GetRssDataFromSourcesAsync(token), "0 * * * *");

            RecurringJob.AddOrUpdate("GetArticlesWithoutTextAndUpdateText",
                () => GetArticlesWithoutTextAndUpdateTextAsync(token), "15 * * * *");

            RecurringJob.AddOrUpdate("RateArticlesWithoutRate",
                () => RateArticlesWithoutRate(token), "30 * * * *");
        }

        public async Task GetRssDataFromSourcesAsync(CancellationToken token)
        {
            try
            {
                var sources = await _sourceService.GetSourcesAsync(token);
                //if (sources != null)
                //{
                //	var tasks = new List<Task>();
                //	foreach (var source in sources)
                //	{
                //		tasks.Add(GetRssDataAsync(source, token));
                //	}
                //	await Task.WhenAll(tasks);
                //}

                if (sources != null)
                {
                    foreach (var source in sources)
                    {
                        await GetRssDataAsync(source, token);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task GetArticlesWithoutTextAndUpdateTextAsync(CancellationToken token)
        {
            //var articles = await GetArticlesWithoutTextAsync(token);
            var articles = (await _mediator.Send(new GetArticlesQuery(), token))
                    .Where(article => string.IsNullOrEmpty(article.Text)).ToArray();
            foreach (var article in articles)
            {
                await UpdateTextAsync(article, token);
            }
        }

        public async Task RateArticlesWithoutRate(CancellationToken token)
        {
            var articles = (await _mediator.Send(new GetArticlesQuery(), token))
                .Where(article => !article.Rate.HasValue).ToArray();
            foreach (var article in articles)
            {
                var newRate = await _articleRateService.CalculateArticleRate(article.Text, token);
                if (newRate != null)
                {
                    SetRateAsync(article.ArticleDtoId, (double)newRate, token);
                }
                else
                {
                    _logger.LogError($"article {article.ArticleDtoId} can not be rated");
                }
            }
        }
        //How to write tests for this method if it accesses a web page from the Internet?
        /*private*/
        public async Task GetRssDataAsync(SourceDto sourceDto, CancellationToken token)
        {
            try
            {
                if (sourceDto.SourceName == "НАКН" || sourceDto.SourceName == "pogoda")
                {
                    return;
                }
                if (sourceDto.RssUrl != null)
                {
                    using (var xmlReader = XmlReader.Create(sourceDto.RssUrl))
                    {
                        var syndicationFeed = SyndicationFeed.Load(xmlReader);

                        var allCategories = syndicationFeed.Items
                            .Select(it => it.Categories.First().Name) //it.Categories is collection but has only 1 element
                            .ToArray();

                        var allCategories2 = syndicationFeed.Items
                            .Select(it => it.Categories.First().Name) //it.Categories is collection but has only 1 element
                            .Distinct()
                            .ToArray();

                        var disC = allCategories.Distinct().ToList();


                        await _mediator.Send(new InsertUniqueCategoriesFromRssDataCommand()
                        {
                            CategoriesNames = allCategories
                        }, token);

                        var categories = await _mediator.Send(new GetCategoriesQuery(), token);
                        Dictionary<string, Guid> categoriesNames = new Dictionary<string, Guid>();

                        foreach (var category in categories)
                        {
                            categoriesNames.Add(category.CategoryName, category.CategoryDtoId);
                        }

                        var articles = syndicationFeed.Items.Select(it => new ArticleDto()
                        {
                            ArticleDtoId = Guid.NewGuid(),
                            //CategoryId = _context.Categories.Where(c => c.CategoryName.Equals(it.Categories.First().Name)).Select(cat => cat.CategoryId).First(),
                            CategoryId = categoriesNames[it.Categories.First().Name],
                            Title = it.Title.Text.Trim(),
                            OriginalUrl = it.Id,
                            Description = it.Summary?.Text.Trim(),
                            PublicationDate = it.PublishDate.DateTime,
                            SourceId = sourceDto.SourceDtoId
                        }).ToArray();

                        await _mediator.Send(
                            new InsertUniqueArticlesFromRssDataCommand()
                            {
                                Articles = articles
                            }, token);

                        _logger.LogInformation($"Got Rss Data from {sourceDto.RssUrl}");

                        //failed attempt to make an article with multiple categories

                        //var existedCategoriesNames = await _context.Categories
                        //    .Select(c => c.CategoryName)
                        //    .ToArrayAsync();
                        //var allCategories = syndicationFeed.Items
                        //    .Select(it => it.Categories.Select(ic =>ic.Name))
                        //    .ToArray();
                        //var newUniqueCat = allCategories
                        //    .Where(c => !existedCategoriesNames.Equals(c))
                        //    .GroupBy(c=>c)
                        // .ToHashSet();
                        //var cat = syndicationFeed.Items.Select(it => 
                        //    it.Categories.Select(ic => new ArticleCategories() { 
                        //        ArticleId = articles.Where(a => a.OriginalUrl ==it.Id).Select(a=>a.ArticleId).First(), 
                        //        CategoryId = _context.Categories.Where(c=>c.CategoryName==ic.Name).Select(c => c.CategoryId).First()}))
                        //        .ToArray();                        
                    }
                }
                else
                {
                    _logger.LogWarning($"Can't GetRssData. source RssUrl is null. SourceName is {sourceDto.SourceName}"); //is it trace or debug level?
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //private async Task<ArticleDto[]> GetArticlesWithoutTextAsync(CancellationToken token)
        //{
        //	try
        //	{
        //		return (await _mediator.Send(new GetArticlesQuery(), token))
        //			.Where(article => string.IsNullOrEmpty(article.Text)).ToArray();
        //	}
        //	catch (Exception e)
        //	{
        //		throw;
        //	}
        //}

        private async Task UpdateTextAsync(ArticleDto articleDto, CancellationToken token)
        {
            var nodePathes = new string[] 
            {
                "//div[@class = 'entry-article__article-wrapper']",//goha
                "//div[@class='news-text']", //onliner
                "//div[@class='full-news-content']", //euroline
            };
                                    
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(articleDto.OriginalUrl, token);
                var contentBytes = await response.Content.ReadAsByteArrayAsync();

                string encodingType;
                if (articleDto.SourceName == "Euroline")
                {
                    encodingType = "windows-1251";
                }
                else
                {
                    encodingType = "utf-8";
                }
                var encoding = Encoding.GetEncoding(encodingType);
                var content = encoding.GetString(contentBytes);

                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                foreach (var path in nodePathes)
                {
                    var articleNode = doc.DocumentNode.SelectSingleNode(path);
                    if (articleNode != null)
                    {
                        await _mediator.Send(new UpdateTextByWebScrappingCommand()
                        {
                            ArticleId = articleDto.ArticleDtoId,
                            NewText = articleNode.InnerText.Trim()
                        }, token);
                        break;
                    }
                }
            }
        }

        public async Task SetRateAsync(Guid id, double newRate, CancellationToken token)
        {
            await _mediator.Send(new SetRateCommand() { ArticleId = id, newRate = newRate }, token);
        }

        public async Task<ArticleWithCommentsModel> GetDetailsAsync(Guid id, CancellationToken token)
        {
            return new ArticleWithCommentsModel()
            {
                Article = ArticleMapper.ArticleDtoToArticleModel
                    (await GetArticleByIdAsync(id, token)),
                Comments = await _mediator.Send(new GetCommentsOnArticleQuery() { ArticleId = id }, token)
            };
        }


    }
}
