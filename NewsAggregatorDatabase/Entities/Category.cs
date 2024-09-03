using NewsAggregatorDatabase.Entities;

namespace NewsAggregatorApp.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Article> Articles { get; set; }
        
        
        //failed attempt to make an article with multiple categories
        //public ICollection<ArticleCategories> ArticleCategories { get; set; }        
    }
}
