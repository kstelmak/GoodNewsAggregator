using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Articles
{
    public class UpdateTextByWebScrappingCommand : IRequest
    {
        //public ArticleDto Article { get; set; }
        public Guid ArticleId { get; set; }
        public string NewText { get; set; }
    }
}
