using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
	internal class SetRoleCommandHandler : IRequestHandler<SetRoleCommand>
	{
		private readonly AggregatorContext _dbContext;
		private readonly ILogger<SetRoleCommandHandler> _logger;

		public SetRoleCommandHandler(AggregatorContext dbContext, ILogger<SetRoleCommandHandler> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task Handle(SetRoleCommand command, CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users.SingleOrDefaultAsync(u=>u.UserId.Equals(command.UserId), cancellationToken);
			user.RoleId = (await _dbContext.Roles.SingleOrDefaultAsync(r => r.RoleName.Equals(command.RoleName))).RoleId;
			await _dbContext.SaveChangesAsync(cancellationToken);
			_logger.LogInformation($"Role by User {user.Name} Chaged to {command.RoleName}");
		}
	}
}
