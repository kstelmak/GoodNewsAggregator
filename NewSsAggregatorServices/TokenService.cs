using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsAggregatorCQS.Queries.User;
using NewsAggregatorServices.Abstractions;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorServices
{
	public class TokenService : ITokenService
	{
		private readonly IMediator _mediator;
		private readonly IConfiguration _configuration;

		public TokenService(IMediator mediator, IConfiguration configuration)
		{
			_mediator = mediator;
			_configuration = configuration;
		}

		public async Task<string> GenerateJwtTokenString(Guid userId, string role, CancellationToken token = default)
		{
			var user = (await _mediator.Send(new GetUsersQuery() { }, token)).Where(u => u.UserDtoId.Equals(userId)).FirstOrDefault();
			var userEmail = user.Email;
			var userName = user.Name;

			var claims = new List<Claim>()
		{
            //new Claim("id", model.Email),
            new Claim(ClaimTypes.Email, userEmail),
			new Claim(ClaimTypes.Name, userName),
			new Claim(ClaimTypes.Role, role),
		};

			//should be a part of service logic(tokenservice for ex.)
			var jwtHandler = new JwtSecurityTokenHandler();
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
			//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jwt:Secret"));


			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				//Issuer = _configuration["Jwt:Iss"],
				//Audience = _configuration["Jwt:Aud"],
				Issuer = _configuration["Jwt:Iss"],
				Audience = _configuration["Jwt:Aud"],
				Expires = DateTime.UtcNow.AddMinutes(15),
				SigningCredentials =
					new SigningCredentials(key,
						SecurityAlgorithms.HmacSha256Signature)
			};

			var jwtToken = jwtHandler.CreateToken(tokenDescriptor);
			var tokenString = jwtHandler.WriteToken(jwtToken);
			return tokenString;
		}

		public async Task<string> GenerateRefreshToken(Guid userId, string deviceInfo)
		{
			//var refreshToken = await _mediator.Send(new CreateRefreshTokenCommand()
			//{
			//	UserId = userId
			//});
			//return refreshToken.ToString("D");
			return "";
		}
		public async Task RemoveRefreshToken(Guid tokenId)
		{
			//_mediator.Send(new RemoveRefreshTokenCommand()
			//{
			//    UserId = userId
			//})

		}

		public async Task RevokeToken(Guid refreshTokenId)
		{

		}

		public async Task<bool> RefreshTokenCorrect(Guid tokenId, CancellationToken cancellationToken = default)
		{
			//var rToken = await _mediator.Send(new GetRefreshTokenByIdQuery() { Id = tokenId }, cancellationToken);
			//return rToken
			//	is { IsRevoked: false }
			//	   && (rToken.ExpireDateTime <= DateTime.UtcNow || rToken.ExpireDateTime == null);
			return false;
		}

		public async Task RemoveToken(Guid id)
		{
		}
	}
}
