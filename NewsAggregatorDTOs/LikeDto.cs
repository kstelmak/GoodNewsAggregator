using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorDTOs
{
    public class LikeDto
    {
        public Guid LikeDtoId { get; set; }
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
    }
}
