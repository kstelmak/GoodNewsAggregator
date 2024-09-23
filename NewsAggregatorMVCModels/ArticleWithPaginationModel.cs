namespace NewsAggregatorMVCModels
{
    public class ArticleWithPaginationModel
    {
        public ArticlePreviewModel[] Articles { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}
