using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMVCModels
{
    public class ArticleWithCommentsModel
    {
        public ArticleModel? Article { get; set; }
        public CommentModel?[] Comments { get; set; }
    }
}
