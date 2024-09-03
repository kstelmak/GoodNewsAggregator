using NewsAggregatorApp.Entities;
using Riok.Mapperly.Abstractions;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;

namespace NewsAggregatorApp.Mappers
{
    [Mapper]
    public static partial class ArticleMapper
    {
        [MapProperty(nameof(Article.ArticleId), nameof(ArticleDto.ArticleDtoId))]
        [MapperIgnoreTarget(nameof(ArticleDto.LikesCount))]
        [MapProperty([nameof(Article.Source), nameof(Article.Source.SourceName)],[nameof(ArticleDto.SourceName)])]

        //[MapProperty([nameof(Article.Source), nameof(Article.Source.SourceName)],[nameof(ArticleDto.SourceName)])]
        public static partial ArticleDto? ArticleToArticleDto(Article? article);

        [MapProperty(nameof(ArticleDto.ArticleDtoId), nameof(ArticleModel.ArticleModelId))]
        public static partial ArticleModel? ArticleDtoToArticleModel(ArticleDto? articleDto);

        [MapProperty(nameof(ArticleDto.ArticleDtoId), nameof(Article.ArticleId))]
        public static partial Article? ArticleDtoToArticle(ArticleDto? articleDto);

        [MapProperty(nameof(ArticleModel.ArticleModelId), nameof(ArticleDto.ArticleDtoId))]
        public static partial ArticleDto? ArticleModelToArticleDto(ArticleModel? articleModel);

        [MapProperty(nameof(ArticleModel.ArticleModelId), nameof(Article.ArticleId))]
        public static partial Article? ArticleModelToArticle(ArticleModel? articleModel);

        [MapProperty(nameof(Article.ArticleId), nameof(ArticleModel.ArticleModelId))]
        public static partial ArticleModel? ArticleToArticleModel(Article? article);

        [MapProperty(nameof(LikeDto.LikeDtoId), nameof(Like.LikeId))]
        public static partial Like? LikeDtoToLike(LikeDto? likeDto);

        [MapProperty(nameof(Like.LikeId), nameof(LikeDto.LikeDtoId))]
        public static partial LikeDto? LikeToLikeDto(Like? like);
    }
}
