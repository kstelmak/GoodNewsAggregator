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

        /// <summary>
        /// Gets articles or article details
        /// </summary>
        /// <remarks>
        /// Gets articles by pageNumber and pageSize if they're not null.
        /// Gets article details by id if it's not null.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
		/// <response code="400">Request is invalid</response>
		/// <response code="404">Article with this id not found</response>
        [HttpGet]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, Guid? id, CancellationToken token = default)
        {
            if (pageSize.HasValue && pageNumber.HasValue)//index
            {
                int minRate = -5;
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    minRate = (await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name, token)).MinRate;
                }
                return Ok(await _articleService.GetArticlesAsync((int)pageNumber, (int)pageSize, minRate, token));
            }
            else if (id != null)//details
            {
                var model = await _articleService.GetDetailsAsync((Guid)id, token);

                if (model.Article != null)
                {
                    var like = (await _userService.GetUserLikesAsync(HttpContext.User.Identity.Name, token))
                        .Where(l => l.ArticleId == id).FirstOrDefault();
                    if (like != null)
                    {
                        model.Article.IsLiked = true;
                    }
                    return Ok(model);
                }
                return NotFound("Article with this id not found");
            }
            return BadRequest();
        }

        /// <summary>
        /// Creates article. Admin only
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Article created successfully</response>
        /// <response code="400">Request is invalid</response>
        /// <response code="401">Unauthorized. Admin only</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        //Create article
        public async Task<IActionResult> Post(ArticleModel articleModel, CancellationToken token = default)
        {
            if (ModelState.IsValid)
            {
                await _articleService.AddArticleAsync(ArticleMapper.ArticleModelToArticleDto(articleModel), token);
                return Created();
            }
            return BadRequest();
        }

        /// <summary>
        /// Updates article. Admin only
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Article updated successfully</response>
        /// <response code="400">Request is invalid</response>
        /// <response code="401">Unauthorized. Admin only</response>
        /// <response code="404">Article with this id not found</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(ArticleModel articleModel, CancellationToken token = default)
        {
            if (ModelState.IsValid)
            {
                if (await _articleService.GetArticleByIdAsync(articleModel.ArticleModelId, token) != null)
                {
                    await _articleService.EditArticleAsync(ArticleMapper.ArticleModelToArticleDto(articleModel), token);
                    return Ok();
                }
                return NotFound("Article with this id not found");
            }
            return BadRequest();
        }

        /// <summary>
        /// Deletes article. Admin only
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Article deleted successfully</response>
        /// <response code="400">Request is invalid</response>
        /// <response code="401">Unauthorized. Admin only</response>
        /// <response code="404">Article with this id not found</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken token = default)
        {
            if (id != null)
            {
                if (await _articleService.GetArticleByIdAsync(id, token) != null)
                {
                    await _articleService.DeleteArticleAsync(id, token);
                    return Ok();
                }
                return NotFound("Article with this id not found");
            }
            return BadRequest();
        }
    }
}
