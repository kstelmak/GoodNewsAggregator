﻿using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Articles
{
    public class GetArticleByIdQuery : IRequest<ArticleDto>
    {
        public Guid ArticleId { get; set; }
    }
}
