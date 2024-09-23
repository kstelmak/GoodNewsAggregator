using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
	internal class UnBlockUserCommandHandler : IRequestHandler<UnBlockUserCommand>
	{
		private readonly AggregatorContext _dbContext;
		private readonly ILogger<UnBlockUserCommandHandler> _logger;

		public UnBlockUserCommandHandler(AggregatorContext dbContext, ILogger<UnBlockUserCommandHandler> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task Handle(UnBlockUserCommand command, CancellationToken cancellationToken)
		{
			var user = _dbContext.Users.Where(u => u.Email.Equals(command.Email)).First();
			user.IsBlocked = false;
			user.UnblockTime = null;
			await _dbContext.SaveChangesAsync(cancellationToken);
			_logger.LogInformation($"User {user.Name} unblocked");
		}
	}
}
