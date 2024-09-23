using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
    internal class BlockUserCommandHandler : IRequestHandler<BlockUserCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<BlockUserCommandHandler> _logger;

        public BlockUserCommandHandler(AggregatorContext dbContext, ILogger<BlockUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(BlockUserCommand command, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.Where(u=>u.Email.Equals(command.Email)).First();
            user.IsBlocked = true;
            user.UnblockTime = command.unblockTime;
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User {user.Name} blocked until {command.unblockTime}");
        }
    }
}
