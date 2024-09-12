using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Mappers;
using NewsAggregatorApp.Services;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using System.Drawing.Printing;

namespace NewsAggregatorApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArticleController : ControllerBase
	{
		private readonly IArticleService _articleService;
        private readonly IUserService _userService;
        private readonly ILogger<ArticleController> _logger;

		public ArticleController(IArticleService articleService, IUserService userService, ILogger<ArticleController> logger)
		{
			_articleService = articleService;
			_userService = userService;
			_logger = logger;
		}

		[HttpGet]
        //gets articles by pageSize & pageNumber or gets article details
        public async Task<IActionResult> Get(Guid? id, int? pageNumber, int? pageSize, CancellationToken token = default)
		{			
            if (pageSize.HasValue && pageNumber.HasValue)//index
            {
                int minRate = -5;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    minRate = (await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name, token)).MinRate;
                }
				return Ok(await _articleService.GetArticlesAsync(pageNumber, pageSize, minRate, token));
            }
			else if (id != Guid.Empty)//details
			{
                var model = await _articleService.GetDetailsAsync((Guid)id, token);

				if(model != null)
				{
                    var like = (await _userService.GetUserLikesAsync(HttpContext.User.Identity.Name, token))
						.Where(l => l.ArticleId == id).FirstOrDefault();
                    if (like != null)
                    {
                        model.Article.IsLiked = true;
                    }
                    return Ok(model);
                }
				else
				{
					return NotFound();
				}
            }
			return BadRequest();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
        //Create article
        public async Task<IActionResult> Post(ArticleModel articleModel, CancellationToken token = default)
		{
			if(ModelState.IsValid)
			{
                await _articleService.AddArticleAsync(ArticleMapper.ArticleModelToArticleDto (articleModel), token);
                return Created();
            }
            return BadRequest();
        }

		[HttpPut]
		[Authorize(Roles = "Admin")]
        //edit article
        public async Task<IActionResult> Put(ArticleModel articleModel, CancellationToken token = default)
		{
			if (ModelState.IsValid)
			{
				await _articleService.EditArticleAsync(ArticleMapper.ArticleModelToArticleDto(articleModel), token);
				return Ok();
			}
			return BadRequest();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
        //Delete article
        public async Task<IActionResult> Delete(Guid id, CancellationToken token = default)
		{
			await _articleService.DeleteArticleAsync(id, token);
			return Ok();
		}
	}
}
