namespace NewsAggregatorApp.Entities
{
    public class Like
    {
        public Guid LikeId { get; set; }
        public Guid ArticleId { get; set; }

        public Article Article { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
