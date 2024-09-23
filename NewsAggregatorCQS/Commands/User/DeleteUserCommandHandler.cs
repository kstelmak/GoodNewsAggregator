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
    internal class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<DeleteUserCommand> _logger;

        public DeleteUserCommandHandler(AggregatorContext dbContext, ILogger<DeleteUserCommand> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Name == command.Username);
            _dbContext.Users.Remove(user); 
            _logger.LogInformation($"Removed user. Username = {command.Username}");
            await _dbContext.SaveChangesAsync();
        }
    }
}
