using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorDTOs
{
    public class SourceDto
    {
        public Guid SourceDtoId { get; set; }
        public string SourceName { get; set; }
        public string? BaseUrl { get; set; }
        public string? RssUrl { get; set; }
    }
}
