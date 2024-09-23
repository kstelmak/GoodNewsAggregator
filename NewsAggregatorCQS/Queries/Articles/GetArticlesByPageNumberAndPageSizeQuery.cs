using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Queries.Articles
{
	public class GetArticlesByPageNumberAndPageSizeQuery : IRequest<ArticleDto[]>
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
		public int MinRate { get; set; }
	}
}
