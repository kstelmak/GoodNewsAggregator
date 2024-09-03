using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorCQS.Commands.Categories;
using NewsAggregatorDTOs;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.User
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(AggregatorContext dbContext, ILogger<AddUserCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(AddUserCommand command, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(UserMapper.RegisterUserDtoToUser(command.registerUserDto), cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User {command.registerUserDto.Name} registered");
        }
    }
}
