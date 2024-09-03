using MediatR;
using Microsoft.Extensions.Logging;
using NewsAggregatorApp.Entities;
using NewsAggregatorApp.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Like
{
    internal class LikeCommandHandler : IRequestHandler<LikeCommand>
    {
        private readonly AggregatorContext _dbContext;
        private readonly ILogger<LikeCommandHandler> _logger;

        public LikeCommandHandler(AggregatorContext dbContext, ILogger<LikeCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(LikeCommand command, CancellationToken cancellationToken)
        {
            var existedLike = _dbContext.Likes.Where(l => l.UserId == command.likeDto.UserId && l.ArticleId == command.likeDto.ArticleId).SingleOrDefault();
            if (existedLike == null)
            {
                var like = ArticleMapper.LikeDtoToLike(command.likeDto);
                await _dbContext.Likes.AddAsync(like);
                _logger.LogInformation($"Added like. LikeId = {like.LikeId}"); //is it ok to put id in logs? 
            }
            else
            {
                _dbContext.Likes.Remove(existedLike);
                _logger.LogInformation($"Removed like. LikeId = {existedLike.LikeId}"); //is it ok to put id in logs?

            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
