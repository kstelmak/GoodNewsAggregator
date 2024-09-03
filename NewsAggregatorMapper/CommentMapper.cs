using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using NewsAggregatorMVCModels;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMapper
{
    [Mapper]
    public static partial class CommentMapper
    {
        [MapProperty(nameof(Comment.CommentId), nameof(CommentModel.CommentModelId))]
        [MapProperty([nameof(Comment.User), nameof(Comment.User.Name)], [nameof(CommentModel.UserName)])]
        public static partial CommentModel? CommentToCommentModel(Comment? comment);



        [MapProperty(nameof(CommentModel.CommentModelId), nameof(Comment.CommentId))]
        public static partial Comment? CommentModelToComment(CommentModel? commentModel);
    }
}
