﻿using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Sourses
{
    public class GetSourcesQuery : IRequest<SourceDto[]>
    {
    }
}
