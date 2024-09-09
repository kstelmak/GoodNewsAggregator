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
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var articles = (await _articleService.GetArticlesAsync(pageNumber, pageSize))                    
                    /*.Select(ArticleMapper.ArticleToArticleDto)
                    .ToArray()*/;

                var articlesCount = await _articleService.GetArticlesCountAsync();

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
        public async Task<IActionResult> Create()
        {
            try
            {
               var model = new ArticleWithCategoriesAndSourcesModel()
                {
                    Article = new ArticleModel(),
                    Categories = await _categoryService.GetCategoriesIdsAndNamesAsync(),
                    Sources = await _sourceService.GetSourcesIdsAndNamesAsync()
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
        public async Task<IActionResult> Create(ArticleWithCategoriesAndSourcesModel model)
        {
            model.Article.ArticleModelId = Guid.NewGuid();
            if (ModelState.IsValid) 
            {
                try
                {                   
                    var modelDto = ArticleMapper.ArticleModelToArticleDto(model.Article);
                    await _articleService.AddArticleAsync(modelDto);
                    
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
                model.Categories = await _categoryService.GetCategoriesIdsAndNamesAsync(); //when the model returns from view Categories and Sources are null. How can this be fixed?
                model.Sources = await _sourceService.GetSourcesIdsAndNamesAsync();
                return View(model);
            }           
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var model = new ArticleWithCategoriesAndSourcesModel()
                {
                    Article = ArticleMapper.ArticleDtoToArticleModel
                                (await _articleService.GetArticleByIdAsync(id)),
                    Categories = await _categoryService.GetCategoriesIdsAndNamesAsync(),
                    Sources = await _sourceService.GetSourcesIdsAndNamesAsync()
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
        public async Task<IActionResult> Edit(ArticleWithCategoriesAndSourcesModel model)
        {
            try
            {
                await _articleService.EditArticleAsync(ArticleMapper.ArticleModelToArticleDto(model.Article));
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _articleService.DeleteArticleAsync(id);
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
        public async Task<IActionResult> Like(Guid articleId, string username)
        {
            await _articleService.LikeAsync(articleId,  username);
            return RedirectToAction("Details", new { id = articleId }); 
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Comment(CommentModel model)
        {
            try
            {
                await _userService.AddCommentAsync(model, HttpContext.User.Identity.Name);
                return RedirectToAction("Details", new { id = model.ArticleId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //show top 3 articles with the highest rating for week
        [HttpGet]
        public async Task<IActionResult> TopArticles()
        {
            //var articles = (await _articleService.GetTopAsync(3))
            //    .Select(ConvertArticleToArticleModel)
            //    .ToArray();

            return View(/*articles*/);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
				var model = await _articleService.GetDetailsAsync(id);
                var like = (await _userService.GetUserLikesAsync(HttpContext.User.Identity.Name)).Where(l=>l.ArticleId==id).FirstOrDefault();
                if(like!=null)
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
        public async Task<IActionResult> Aggregate()
        {
            try
            {
                CancellationToken token = default;
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
