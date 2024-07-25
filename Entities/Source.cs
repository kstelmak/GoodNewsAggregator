namespace NewsAggregatorApp.Entities
{
    public class Source
    {
        public Guid SourceId { get; set; }
        public string SourceName { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
