using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorDTOs;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace NewsAggregatorApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AggregationController : ControllerBase
	{
		private readonly IArticleService _articleService;
		private readonly ISourceService _sourceService;
		private readonly ILogger<AggregationController> _logger;

		public AggregationController(IArticleService articleService, ISourceService sourceService, ILogger<AggregationController> logger)
		{
			_articleService = articleService;
			_sourceService = sourceService;
			_logger = logger;
		}

        /// <summary>
        /// Inicialized article aggregation job. Admin only
        /// </summary>
		/// <remarks>
        /// Starts article aggregation job if aggregateNow is true. By default aggregateNow is true.
        /// Inicialized article aggregation background job if aggregateNow is false:
		/// at minute 0 gets rss data; at minute 15 updates article text; at minute 30 updates article rate
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
		/// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Aggregate(bool aggregateNow = true, CancellationToken token = default)
		{
			if (aggregateNow)
			{
                _articleService.AggregateAsync(token);
                return Ok("Aggregation inicialized");
            }
			else
			{
                _articleService.AggregateInBackgroundAsync(token);
                return Ok("Background Jobs inicialized");
            }
		}
	}
}
