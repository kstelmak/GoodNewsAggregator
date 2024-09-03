namespace NewsAggregatorMVCModels
{
    public class ArticleWithCategoriesAndSourcesModel
    {
        public ArticleModel Article { get; set; }
        public Dictionary<Guid, string>? Categories { get; set; }
        public Dictionary<Guid, string>? Sources { get; set; }

    }
}
