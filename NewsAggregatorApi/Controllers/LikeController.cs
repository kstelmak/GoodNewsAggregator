using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services.Abstractions;

namespace NewsAggregatorApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IUserService _userService;
        private readonly ILogger<LikeController> _logger;

        public LikeController(IArticleService articleService, IUserService userService, ILogger<LikeController> logger)
        {
            _articleService = articleService;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(Guid articleId, string username, CancellationToken token = default)
        {
            if(articleId!=Guid.Empty && !String.IsNullOrEmpty(username))
            {
                if (await _articleService.GetArticleByIdAsync(articleId, token) != null)
                {
                    await _userService.LikeAsync(articleId, username, token);
                    return Ok();
                }
                return NotFound("article with such id does not exist");
            }
            return BadRequest();
        }
    }
}
