using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorServices.Abstractions
{
	public interface ITokenService
	{
		public Task<string> GenerateJwtTokenString(Guid userId, string role, CancellationToken token = default);
		public Task<string> GenerateRefreshToken(Guid userId, string deviceInfo);
		public Task RevokeToken(Guid refreshTokenId);
		Task<bool> RefreshTokenCorrect(Guid tokenId, CancellationToken cancellationToken = default);
		Task RemoveToken(Guid id);
	}
}
