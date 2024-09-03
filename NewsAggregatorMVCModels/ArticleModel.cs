using System.ComponentModel.DataAnnotations;

namespace NewsAggregatorMVCModels
{
    public class ArticleModel
    {
        [Required]
        public Guid ArticleModelId { get; set; }
        [Required]
        public Guid? CategoryId { get; set; }
        [Required]
        [MinLength(3)]
        public string Title { get; set; }
        //[MaxLength(1000)]
        public string? Description { get; set; }
        [Required]
        [MinLength(3)]
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }
        [Range(-5, 5, ErrorMessage = "value must be between -5 and 5")]
        public double? Rate { get; set; }
        [Required]
        public Guid SourceId { get; set; }
        public string? SourceName { get; set; }
        public int? LikesCount { get; set; }
        public bool? IsLiked { get; set; }
    }
}
