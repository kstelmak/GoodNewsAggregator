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
    internal class EditUserCommandHandler : IRequestHandler<EditUserCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<EditUserCommandHandler> _logger;

        public EditUserCommandHandler(AggregatorContext dbContext, ILogger<EditUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(EditUserCommand command, CancellationToken cancellationToken)
        {
            var user = _dbContext.Users.Where(u => u.UserId.Equals(command.UserDto.UserDtoId)).First();
            user.Name = command.UserDto.Name;
            user.MinRate = command.UserDto.MinRate;
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User {command.UserDto.Email} changed");
        }
    }
}
