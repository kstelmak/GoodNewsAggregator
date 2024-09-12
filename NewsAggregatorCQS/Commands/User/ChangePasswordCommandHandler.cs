using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(AggregatorContext dbContext, ILogger<ChangePasswordCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email.Equals(command.Email), cancellationToken);
            user.PasswordHash = command.PasswordHash;
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Password by User {user.Name} Chaged");
        }
    }
}
