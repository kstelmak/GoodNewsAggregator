using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorMVCModels;
using NewsAggregatorServices.Abstractions;

namespace NewsAggregatorApi.Controllers
{
	public class TokenController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly ITokenService _tokenService;

		public TokenController(/*IConfiguration configuration,*/ IUserService userService, ITokenService tokenService)
		{
			//_configuration = configuration;
			_userService = userService;
			_tokenService = tokenService;
		}

		//todo add model for refreshToken
		[HttpPost("/refresh/")]
		public async Task<IActionResult> Refresh([FromBody] Guid refreshTokenId,
			CancellationToken cancellationToken = default)
		{
			if (await _tokenService.RefreshTokenCorrect(refreshTokenId, cancellationToken))
			{
				var user = await _userService.GetUserDataByRefreshToken(refreshTokenId, cancellationToken);
				if (user != null)
				{
					var data = await GenerateTokenPair(user.Id, user.RoleName);
					await _tokenService.RemoveToken(refreshTokenId);
					return Ok(new { AccessToken = data.Item1, refreshToken = data.Item2 });
				}
			}
			return NotFound();
		}

		[HttpPost("/login")]
		public async Task<IActionResult> Login([FromBody] UserLoginModel model,
			CancellationToken cancellationToken = default)
		{
			if (!await _userService.CheckIsEmailRegisteredAsync(model.Email, cancellationToken) ||
				!await _userService.CheckPasswordAsync(model.Email, model.Password, cancellationToken))
			{
				return Unauthorized();
			}

			var userRole = await _userService.GetUserRoleNameByEmailAsync(model.Email, cancellationToken);
			var userId = await _userService.GetUserIdByEmailAsync(model.Email, cancellationToken);

			if (string.IsNullOrWhiteSpace(userRole) || userId==Guid.Empty)
			{
				return StatusCode(500);
			}

			var data = await GenerateTokenPair(userId, userRole);
			return Ok(new { AccessToken = data.Item1, RefreshToken = data.Item2 });
		}

		[HttpPatch("/revoke/{id}")]
		public async Task<IActionResult> Revoke(Guid id,
			CancellationToken cancellationToken = default)
		{
			await _tokenService.RevokeTokenAsync(id);
			return NotFound();
		}

		private async Task<(string, string)> GenerateTokenPair(Guid userId, string userRole)
		{
			var jwt = await _tokenService.GenerateJwtTokenStringAsync(userId, userRole);
			var deviceInfo = "localhost";
			var rt = await _tokenService.GenerateRefreshTokenAsync(userId, deviceInfo);
			return (jwt, rt);
		}
	}
}

