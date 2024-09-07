using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NewsAggregatorApp.Services;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorCQS.Commands.Articles;
using NewsAggregatorCQS.Commands.Categories;
using NewsAggregatorCQS.Queries.Articles;
using NewsAggregatorDTOs;
using Xunit;
using NewsAggregatorCQS.Queries.Categories;
using Microsoft.IdentityModel.Tokens;
using NewsAggregatorApp.Mappers;
namespace NewsAggregatorTests
{
	public class ArticleServiceTests
	{
		private readonly ISourceService _sourceServiceMock;
		private readonly IMediator _mediatorMock;
		private readonly ILogger<ArticleService> _loggerMock;
		private readonly ArticleService _articleService;

		private List<ArticleDto> _articles = [];
		private List<SourceDto> _sources = [];


		private CancellationToken cancellationToken = new CancellationToken();



		//code by ChatGpt
		//it doesn't work and I don't know what to do with requests to web pages

		//public ArticleServiceTests()
		//{
		//	_sources =
		//	[
		//		new SourceDto
		//		{
		//			SourceDtoId = Guid.NewGuid(),
		//			SourceName = "0 Sample Source",
		//			BaseUrl = "https://example0.com",
		//			RssUrl = "https://example0.com/rss",
		//			//Articles = new List<Article>()
		//		},
		//		new SourceDto
		//		{
		//			SourceDtoId = Guid.NewGuid(),
		//			SourceName = "1 Sample Source",
		//			BaseUrl = "https://example1.com",
		//			RssUrl = "https://example1.com/rss",
		//			//Articles = new List<Article>()
		//		},
		//		new SourceDto
		//		{
		//			SourceDtoId = Guid.NewGuid(),
		//			SourceName = "2 Sample Source",
		//			BaseUrl = "https://example2.com",
		//			RssUrl = "https://example2.com/rss",
		//			//Articles = new List<Article>()
		//		},
		//	];

		//	_articles =
		//	[
		//		new ArticleDto
		//		{
		//			ArticleDtoId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
		//			Title = "1 Sample Article",
		//			Description = "1 This is a sample article",
		//			Text = null,
		//			OriginalUrl = "https://example.com/sample-article",
		//			PublicationDate = DateTime.Now,
		//			Rate = 3.5,
		//			SourceId = Guid.NewGuid(),
		//			//Source = _sources[0]
		//		},
		//		new ArticleDto
		//		{
		//			ArticleDtoId = Guid.Parse("22211111-1111-1111-1111-111111111111"),
		//			Title = "2 Sample Article",
		//			Description = "2 This is a sample article",
		//			Text = "2 Lorem ipsum dolor sit amet",
		//			OriginalUrl = "https://example.com/sample-article",
		//			PublicationDate = DateTime.Now,
		//			Rate = 3.5,
		//			SourceId = Guid.NewGuid(),
		//			//Source = _sources[1]
		//		},
		//		new ArticleDto
		//		{
		//			ArticleDtoId = Guid.Parse("33311111-1111-1111-1111-111111111111"),
		//			Title = "3 Sample Article",
		//			Description = "3 This is a sample article",
		//			Text = "3 Lorem ipsum dolor sit amet",
		//			OriginalUrl = "https://example.com/sample-article",
		//			PublicationDate = DateTime.Now,
		//			Rate = 3.5,
		//			SourceId = Guid.NewGuid(),
		//			//Source = _sources[2]
		//		}
		//	];

		//	_sourceServiceMock = Substitute.For<ISourceService>();
		//	_mediatorMock = Substitute.For<IMediator>();
		//	_loggerMock = Substitute.For<ILogger<ArticleService>>();

		//	_articleService = new ArticleService(_loggerMock, _sourceServiceMock, _mediatorMock);
		//	_sourceServiceMock.GetSourcesAsync(cancellationToken)
		//		.ReturnsForAnyArgs(
		//		Task.FromResult(_sources.ToArray()));
		//	_mediatorMock.Send(new GetArticlesQuery(), cancellationToken)
		//		.ReturnsForAnyArgs(
		//		Task.FromResult(_articles.ToArray()));
		//	//_articleService.GetArticlesWithoutTextAsync().ReturnsForAnyArgs(_articles.Where(a=>a.Text.IsNullOrEmpty()).ToArray());
		//}

		//[Fact]
		//public async Task AggregateAsync_SourcesExist_CallsGetRssDataAsync()
		//{
		//	// Arrange
		//	var source = _sources[0];
		//	_articleService.GetRssDataAsync(source, cancellationToken)
		//		.ReturnsForAnyArgs(Task.FromResult(1));

		//	// Act
		//	var s = _sourceServiceMock.GetSourcesAsync(cancellationToken);
		//	await _articleService.AggregateAsync(cancellationToken);

		//	// Assert
		//	await _sourceServiceMock.Received(1).GetSourcesAsync(cancellationToken);
		//	//await _mediatorMock.Received(1).Send(Arg.Any<GetCategoriesQuery>(), Arg.Any<CancellationToken>());
		//}

		//[Fact]
		//public async Task GetRssDataAsync_ValidRssUrl_ProcessesRssFeed()
		//{
		//	// Arrange
		//	var sourceDto = new SourceDto { RssUrl = "http://example.com/rss" };
		//	var categories = new List<CategoryDto>
		//{
		//	new CategoryDto { CategoryName = "Tech", CategoryDtoId = Guid.NewGuid() }
		//};

		//	_mediatorMock.Send(Arg.Any<GetCategoriesQuery>(), Arg.Any<CancellationToken>()).Returns(categories.ToArray());

		//	// Act
		//	var getRssDataAsyncMethod = typeof(ArticleService)
		//		.GetMethod("GetRssDataAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		//	if (getRssDataAsyncMethod != null)
		//	{
		//		await (Task)getRssDataAsyncMethod.Invoke(_articleService, new object[] { sourceDto, CancellationToken.None });
		//	}

		//	// Assert
		//	await _mediatorMock.Received(1).Send(Arg.Any<InsertUniqueCategoriesFromRssDataCommand>(), Arg.Any<CancellationToken>());
		//	await _mediatorMock.Received(1).Send(Arg.Any<InsertUniqueArticlesFromRssDataCommand>(), Arg.Any<CancellationToken>());
		//}

		//[Fact]
		//public async Task GetArticlesWithoutTextAsync_ReturnsArticlesWithoutText()
		//{
		//	// Arrange
		//	var expected = new ArticleDto[] {
		//		_articles[0],_articles[2],
		//	};

		//	// Act
		//	var result = await _articleService.GetArticlesWithoutTextAsync();

		//	// Assert
		//	Assert.Equal(expected, result);
		//}

		//[Fact]
		//public async Task UpdateTextAsync_ArticleWithNoText_UpdatesText()
		//{
		//	// Arrange
		//	var newText = "new text";
		//}

		//[Fact] //just gets articles
		//public async Task Test()
		//{
		//	//Arrange
		//	var articleService = GetArticleServiceWithMocks();
		//	var exp = new ArticleDto[] { ArticleMapper.ArticleToArticleDto(_articles[0]), ArticleMapper.ArticleToArticleDto(_articles[1]) };
		//	//Act
		//	var res = await articleService.GetArticlesAsync(1, 2);
		//	//Assert
		//	Assert.Equal(exp, res);
		//}
	}
}

