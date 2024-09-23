using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMVCModels
{
    public class CommentModel
    {
        public Guid CommentModelId { get; set; }
        [Required(ErrorMessage = "Comment text is required.")]
        public string CommentText { get; set; }
        public Guid ArticleId { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
    }
}
