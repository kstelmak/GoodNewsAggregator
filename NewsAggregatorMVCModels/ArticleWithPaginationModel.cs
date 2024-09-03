namespace NewsAggregatorMVCModels
{
    public class ArticleWithPaginationModel
    {
        public ArticleModel[] Articles { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}
