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

        /// <summary>
        /// Adds comment to article. Authorized users only
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Like created successfully</response>
        /// <response code="401">Unauthorized. Authorized users only</response>
        /// <response code="404">User with this name or article with this id not found</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(Guid articleId, string username, CancellationToken token = default)
        {

            if (await _articleService.GetArticleByIdAsync(articleId, token) == null)
            {
                return NotFound("article with this id not found");
            }
            if (await _userService.GetUserByNameAsync(username, token) == null)
            {
                return NotFound("user with this name not foundt");
            }
            await _userService.LikeAsync(articleId, username, token);
            return Created();
        }
    }
}
