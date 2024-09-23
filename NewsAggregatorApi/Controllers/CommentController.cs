using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(IArticleService articleService, IUserService userService, ILogger<CommentController> logger)
        {
            _articleService = articleService;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Adds comment to article. Authorized users only
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Comment created successfully</response>
        /// <response code="401">Unauthorized. Authorized users only</response>
        /// <response code="404">User with this name or article with this id not found</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CommentModel model, CancellationToken token = default)
        {
            if( await _userService.GetUserByNameAsync(model.UserName, token) == null)
            {
				return NotFound("user with this name not found");
			}
			if (await _articleService.GetArticleByIdAsync(model.ArticleId, token) == null)
			{
				return NotFound("article with this id not found");
			}
			await _userService.AddCommentAsync(model, token);
            return Created();    
        }
    }
}
