﻿using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Articles
{
    public class InsertUniqueArticlesFromRssDataCommand : IRequest
    {
        public ArticleDto[] Articles { get; set; }
    }
}
