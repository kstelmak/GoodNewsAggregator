using Microsoft.AspNetCore.Mvc;
using NewsAggregatorApp.Services.Abstractions;
using NewsAggregatorMVCModels;
using NewsAggregatorServices.Abstractions;
using Newtonsoft.Json.Linq;

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

        /// <summary>
        /// Generates new access token - refresh token pair using refresh token id
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Tokens generated successfully</response>
        /// <response code="404">Refresh token with this id not found</response>
        //todo add model for refreshToken
        [HttpPost("/refresh/")]
		public async Task<IActionResult> Refresh([FromBody] Guid refreshTokenId,
			CancellationToken cancellationToken = default)
		{
			if (await _tokenService.RefreshTokenCorrect(refreshTokenId, cancellationToken))
			{
				var user = await _userService.GetUserDataByRefreshTokenAsync(refreshTokenId, cancellationToken);
				if (user != null)
				{
					var data = await GenerateTokenPair(user.Id, user.RoleName);
					await _tokenService.RemoveToken(refreshTokenId);
					return Ok(new { AccessToken = data.Item1, refreshToken = data.Item2 });
				}
			}
			return NotFound("Refresh token with this id not found");
		}

        /// <summary>
        /// Generates access token - refresh token pair using user eMail and password
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Tokens generated successfully</response>
        /// <response code="401">Unauthorized. Email or password is invalid</response>
		/// <response code="500">Internal server error</response>
        [HttpPost("/login")]
		public async Task<IActionResult> Login([FromBody] UserLoginModel model,
			CancellationToken cancellationToken = default)
		{
			if (!await _userService.CheckIsEmailRegisteredAsync(model.Email, cancellationToken) ||
				!await _userService.CheckPasswordAsync(model.Email, model.Password, cancellationToken))
			{
				return Unauthorized();
            }

			var user = await _userService.GetUserByEmailAsync(model.Email, cancellationToken);

			if (string.IsNullOrWhiteSpace(user.RoleName) || user.UserDtoId == Guid.Empty)
			{
				return StatusCode(500);
			}

			var data = await GenerateTokenPair(user.UserDtoId, user.RoleName);
			return Ok(new { AccessToken = data.Item1, RefreshToken = data.Item2 });
		}

        /// <summary>
        /// Revokes refresh token by id
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Token revoked successfully</response>
        /// <response code="404">Refresh token with this id not found</response>
        [HttpPatch("/revoke/{id}")]
		public async Task<IActionResult> Revoke(Guid id, CancellationToken cancellationToken = default)
		{
			if (await _tokenService.RefreshTokenCorrect(id))
			{
                await _tokenService.RevokeTokenAsync(id);
                return Ok();
            }
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

