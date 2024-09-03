namespace NewsAggregatorApp.Entities
{
    public class Source
    {
        public Guid SourceId { get; set; }
        public string SourceName { get; set; }
        public string? BaseUrl { get; set; }
        public string? RssUrl { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
