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
    }
}
