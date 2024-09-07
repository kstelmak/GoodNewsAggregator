using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorDTOs
{
    public class ArticleDto
    {
        public Guid ArticleDtoId { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? OriginalUrl { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public double? Rate { get; set; }
        public Guid SourceId { get; set; }
        public string? SourceName { get; set; }
        public int? LikesCount { get; set; }

		public override bool Equals(object? obj)
		{
			return obj is ArticleDto other &&
				   (ArticleDtoId, CategoryId, CategoryName, OriginalUrl, Title, Description, Text,
					PublicationDate, Rate, SourceId, SourceName, LikesCount)
				   .Equals((other.ArticleDtoId, other.CategoryId, other.CategoryName, other.OriginalUrl,
							other.Title, other.Description, other.Text,
							other.PublicationDate, other.Rate, other.SourceId,
							other.SourceName, other.LikesCount));
		}
	}
}
