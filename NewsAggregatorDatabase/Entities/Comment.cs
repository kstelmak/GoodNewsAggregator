namespace NewsAggregatorApp.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
