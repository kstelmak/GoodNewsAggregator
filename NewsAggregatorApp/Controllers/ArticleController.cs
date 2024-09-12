using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorApp.Services;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorMVCModels;
using NewsAggregatorDTOs;
using System.Security.Claims;
using System.ServiceModel.Syndication;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace NewsAggregatorApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ISourceService _sourceService;
        private readonly IUserService _userService;
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, ISourceService sourceService, IUserService userService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _sourceService = sourceService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, CancellationToken token = default)
        {
            try
            {
                int minRate = -5;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    minRate = (await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name, token)).MinRate;
                }
                var articles = (await _articleService.GetArticlesAsync(pageNumber, pageSize, minRate, token)).ToArray();

                // var articlesCount = await _articleService.GetArticlesCountAsync();
                var articlesCount = articles.Count();

                var pagination = new PaginationModel()
                {
                    PageSize = pageSize,
                    PageNumber = pageNumber,
                    TotalPages = articlesCount % pageSize == 0
                        ? articlesCount / pageSize
                        : articlesCount / pageSize + 1
                };

                var model = new ArticleWithPaginationModel()
                {
                    Articles = articles.Select(dto => ArticleMapper.ArticleDtoToArticleModel(dto)).ToArray(),
                    Pagination = pagination
                };

                _logger.LogTrace($"Controller: Article Action: Index got {articlesCount} articles"); //such logs make sense?
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CancellationToken token = default)
        {
            try
            {
                var model = new ArticleWithCategoriesAndSourcesModel()
                {
                    Article = new ArticleModel(),
                    Categories = await _categoryService.GetCategoriesIdsAndNamesAsync(token),
                    Sources = await _sourceService.GetSourcesIdsAndNamesAsync(token)
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ArticleWithCategoriesAndSourcesModel model, CancellationToken token = default)
        {
            model.Article.ArticleModelId = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                try
                {
                    var modelDto = ArticleMapper.ArticleModelToArticleDto(model.Article);
                    await _articleService.AddArticleAsync(modelDto, token);

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    return StatusCode(500, new { Message = e.Message });
                }
            }
            else
            {
                model.Categories = await _categoryService.GetCategoriesIdsAndNamesAsync(token); //when the model returns from view Categories and Sources are null. How can this be fixed?
                model.Sources = await _sourceService.GetSourcesIdsAndNamesAsync(token);
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id, CancellationToken token = default)
        {
            try
            {
                var model = new ArticleWithCategoriesAndSourcesModel()
                {
                    Article = ArticleMapper.ArticleDtoToArticleModel
                                (await _articleService.GetArticleByIdAsync(id, token)),
                    Categories = await _categoryService.GetCategoriesIdsAndNamesAsync(token),
                    Sources = await _sourceService.GetSourcesIdsAndNamesAsync(token)
                };
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ArticleWithCategoriesAndSourcesModel model, CancellationToken token = default)
        {
            try
            {
                await _articleService.EditArticleAsync(ArticleMapper.ArticleModelToArticleDto(model.Article), token);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token = default)
        {
            try
            {
                await _articleService.DeleteArticleAsync(id, token);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Like(Guid articleId, string username, CancellationToken token = default)
        {
            try
            {
                await _userService.LikeAsync(articleId, username, token);
                return RedirectToAction("Details", new { id = articleId });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comment(CommentModel model, CancellationToken token = default)
        {
            try
            {
                await _userService.AddCommentAsync(model, token);
                return RedirectToAction("Details", new { id = model.ArticleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //show top 3 articles with the highest rating for week
        //[HttpGet]
        //public async Task<IActionResult> TopArticles()
        //{
        //    //var articles = (await _articleService.GetTopAsync(3))
        //    //    .Select(ConvertArticleToArticleModel)
        //    //    .ToArray();

        //    return View(/*articles*/);
        //}

        [HttpGet]
        public async Task<IActionResult> Details(Guid id, CancellationToken token = default)
        {
            try
            {
                var model = await _articleService.GetDetailsAsync(id, token);
                var like = (await _userService.GetUserLikesAsync(HttpContext.User.Identity.Name, token))
                    .Where(l => l.ArticleId == id).FirstOrDefault();
                if (like != null)
                {
                    model.Article.IsLiked = true;
                }
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Aggregate(CancellationToken token = default)
        {
            try
            {
                await _articleService.AggregateAsync(token);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { Message = e.Message });
            }
        }
    }
}
