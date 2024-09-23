//using BenchmarkDotNet.Running;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using NewsAggregatorApp.Entities;
//using NewsAggregatorApp.Mappers;
//using NewsAggregatorApp.Services;
//using NewsAggregatorApp.Services.Abstractions;
//using NewsAggregatorCQS.Queries.Articles;
//using NewsAggregatorCQS.Queries.User;
//using NewsAggregatorDTOs;
//using NewsAggregatorMapper;
//using NSubstitute;
//using System.Collections;
//using static System.Net.Mime.MediaTypeNames;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace NewsAggregatorTests
//{
//    public class ArticleRateServiceTest
//    {
//        private List<Article> _articles = [];
//		private List<Source> _sources = [];
//        private CancellationToken _cancellationToken = new CancellationToken();

//		private ArticleRateService GetArticleRateServiceWithMocks(Guid articleId, double newRate)
//        {
//            _articles =
//            [
//               new Article
//               {
//                   ArticleId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
//                   Title = "Sample Article",
//                   Description = "This is a sample article",
//                   Text = "Lorem ipsum dolor sit amet",
//                   OriginalUrl = "https://example.com/sample-article",
//                   PublicationDate = DateTime.Now,
//                   Rate = 3.5,
//                   SourceId = Guid.NewGuid(),
//                   Source = new Source
//                   {
//                       SourceId = Guid.NewGuid(),
//                       SourceName = "Sample Source",
//                       BaseUrl = "https://example.com",
//                       RssUrl = "https://example.com/rss",
//                       Articles = new List<Article>()
//                   }
//               }
//            ];
//            var cToken = new CancellationToken();
//            var articleServiceMock = Substitute.For<IArticleService>();

//            articleServiceMock.GetArticleByIdAsync(articleId, _cancellationToken)
//                           .ReturnsForAnyArgs(
//                               Task.FromResult(
//                                   ArticleMapper.ArticleToArticleDto(_articles.FirstOrDefault(article => article.ArticleId.Equals(articleId)))));

//            articleServiceMock.SetRateAsync(articleId, newRate, cToken)
//                .ReturnsForAnyArgs(Task.FromResult(1))
//                .AndDoes(info =>
//                {
//                    _articles[0].Rate = newRate;
//                });

//            var ars = new ArticleRateService(/*articleServiceMock*/);

//            return ars;
//        }

//		[Theory]
//        [InlineData(-2)]
//        [InlineData(3)]
//        [InlineData(4.8)]
//        [InlineData(-5)]
//        [InlineData(5)]
//        [InlineData(0)]
//        public async Task SetArticleRateAsync_UpdateWithCorrectRate_UpdateCorrectly(double newRate)
//        {  
//            var articleId = Guid.Parse("11111111-1111-1111-1111-111111111111");

//            var ars = GetArticleRateServiceWithMocks(articleId, newRate);
//            await ars.SetArticleRateAsync(articleId, newRate);
//            Assert.Equal(newRate, _articles[0].Rate);            
//        }

//        [Theory]
//        [InlineData(double.NegativeInfinity)]
//        [InlineData(-7)]
//        [InlineData(5.1)]
//        [InlineData(50)]
//        [InlineData(double.PositiveInfinity)]
//        public async Task SetArticleRateAsync_UpdateWithIncorrectRate_ThrowException(double newRate)
//        {
//            var articleId = Guid.Parse("11111111-1111-1111-1111-111111111111");
//            const string exMessage = "Incorrect rate (Parameter 'newRate')";

//            var ars = GetArticleRateServiceWithMocks(articleId, newRate);
//            var ex = await Assert.ThrowsAsync(typeof(ArgumentException), () =>
//                ars.SetArticleRateAsync(articleId, newRate));
//            Assert.Equal(exMessage, ex.Message);
//        }

//        [Theory]
//        [InlineData("432C080C-2500-480E-9E0B-27D5416CD4D9")]
//        [InlineData("37435125-C20A-4A3E-8CC2-FEEF17A9C95F")]
//        [InlineData("2793A5C4-F2B6-44AA-9551-0D60C118C198")]
//        public async Task SetArticleRateAsync_UpdateWithIncorrectId_ThrowException(string guidValue)
//        {
           
//            var articleId = Guid.Parse(guidValue);
//            const string exMessage = "Article with that Id doesn't exist (Parameter 'articleId')";
//            var newRate = 0;

//            var ars = GetArticleRateServiceWithMocks(articleId, newRate);            
//            var ex = await Assert.ThrowsAsync(typeof(ArgumentException), () =>
//                ars.SetArticleRateAsync(articleId, newRate));
//            Assert.Equal(exMessage, ex.Message);
//        }
//	}
//}