using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Tokens
{
	internal class RemoveRefreshTokenCommandHandler : IRequestHandler<RemoveRefreshTokenCommand>
	{
		private readonly AggregatorContext _dbContext;
		private readonly ILogger<RemoveRefreshTokenCommandHandler> _logger;

		public RemoveRefreshTokenCommandHandler(AggregatorContext articleAggregatorContext, ILogger<RemoveRefreshTokenCommandHandler> logger)
		{
			_dbContext = articleAggregatorContext;
			_logger = logger;
		}

		public async Task Handle(RemoveRefreshTokenCommand request, CancellationToken cancellationToken)
		{
			var rt = _dbContext.RefreshTokens.Where(rt => rt.RefreshTokenId.Equals(request.Token)).First();
			_dbContext.RefreshTokens.Remove(rt);
			await _dbContext.SaveChangesAsync(cancellationToken);
			_logger.LogInformation($"Token is removed. Id {rt.RefreshTokenId}");
		}
	}
}
