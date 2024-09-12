using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CommentModel model, CancellationToken token = default)
        {
            if( await _userService.GetUserByNameAsync(model.UserName, token) == null)
            {
				return NotFound("user with such id does not exist");
			}
			if (await _articleService.GetArticleByIdAsync(model.ArticleId, token) == null)
			{
				return NotFound("article with such id does not exist");
			}
			await _userService.AddCommentAsync(model, token);
            return Ok();    
        }
    }
}
