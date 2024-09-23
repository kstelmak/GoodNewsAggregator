using NewsAggregatorDatabase.Entities;

namespace NewsAggregatorApp.Entities
{
    public class Article
    {
        public Guid ArticleId { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? OriginalUrl { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Text { get; set; }
        public DateTime PublicationDate { get; set; }
        public double? Rate { get; set; }
        public Guid SourceId { get; set; }
        public Source? Source { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }

        //failed attempt to make an article with multiple categories
        //public ICollection<ArticleCategories> ArticleCategories { get; set; }
    }
}
