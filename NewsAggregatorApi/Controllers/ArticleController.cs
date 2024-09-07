using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Mappers;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorDTOs;
using System.Drawing.Printing;

namespace NewsAggregatorApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArticleController : ControllerBase
	{
		private readonly IArticleService _articleService;
		private readonly ILogger<ArticleController> _logger;

		public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
		{
			_articleService = articleService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Get(int? pageNumber = 1, int? pageSize = 10)
		{
			if (pageSize.HasValue && pageNumber.HasValue)
			{
				return Ok(await _articleService.GetArticlesAsync(pageNumber, pageSize));
			}
			return NotFound();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Post(ArticleDto articleDto)
		{
			//in mvc app Create method receives a model wich is checked using if (ModelState.IsValid){}
			//this method receives DTO
			//is it ok to validate like this??			
			if (articleDto.ArticleDtoId.Equals(Guid.Empty) ||
				articleDto.CategoryId.Equals(Guid.Empty) ||
				articleDto.Title == null ||articleDto.Title.Length<3 ||
				articleDto.Text == null ||articleDto.Text.Length < 3 ||
				articleDto.Rate>5|| articleDto.Rate < -5 ||
				articleDto.SourceId.Equals(Guid.Empty))
			{
				await _articleService.AddArticleAsync(articleDto);
				return Created();
			}
			return BadRequest();
		}

		[HttpPatch]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Patch(ArticleDto articleDto)
		{
			//where should the DTO validation method be placed and how best to write it?
			if (articleDto.ArticleDtoId.Equals(Guid.Empty) ||
				articleDto.CategoryId.Equals(Guid.Empty) ||
				articleDto.Title == null || articleDto.Title.Length < 3 ||
				articleDto.Text == null || articleDto.Text.Length < 3 ||
				articleDto.Rate > 5 || articleDto.Rate < -5 ||
				articleDto.SourceId.Equals(Guid.Empty))
			{
				await _articleService.EditArticleAsync(articleDto);
				return Ok();
			}
			return BadRequest();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _articleService.DeleteArticleAsync(id);
			return Ok();
		}
	}
}
