using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using NewsAggregatorMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Comment
{
    internal class AddCommentCommandHandler : IRequestHandler<AddCommentCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<AddCommentCommandHandler> _logger;

        public AddCommentCommandHandler(AggregatorContext dbContext, ILogger<AddCommentCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(AddCommentCommand command, CancellationToken cancellationToken)
        {
            await _dbContext.Comments.AddAsync(CommentMapper.CommentModelToComment(command.Comment));
            _logger.LogInformation($"Added Comment. CommentsId = {command.Comment.CommentModelId}"); //is it ok to put id in logs?
            await _dbContext.SaveChangesAsync();
        }
    }
}
