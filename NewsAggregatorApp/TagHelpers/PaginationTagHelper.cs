using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NewsAggregatorMVCModels;
using System.Text;

namespace NewsAggregatorApp.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly ILogger<PaginationTagHelper> _logger;

        public PaginationTagHelper(ILogger<PaginationTagHelper> logger)
        {
            _logger = logger;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public PaginationModel Model { get; set; } = new();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Model.TotalPages > 1)
            {
                output.TagName = "nav"; 
                output.TagMode = TagMode.StartTagAndEndTag;
                var content = new StringBuilder();
                var currentRouteUrl = ViewContext.HttpContext.Request.Path;
                int currentPage = Model.PageNumber;
                int totalPages = Model.TotalPages;

                content.AppendLine("<ul class='pagination'>");

                // Первая страница
                if (currentPage > 1)
                {
                    content.AppendLine($"<li class='page-item'><a class='page-link' href='{currentRouteUrl}?pageSize={Model.PageSize}&pageNumber=1'>First</a></li>");
                }

                // Текущая и соседние страницы
                int startPage = Math.Max(1, currentPage - 2);
                int endPage = Math.Min(totalPages, currentPage + 2);

                if (startPage > 1)
                {
                    content.AppendLine("<li class='page-item disabled'><span class='page-link'>...</span></li>");
                }

                for (int i = startPage; i <= endPage; i++)
                {
                    if (i == currentPage)
                    {
                        content.AppendLine($"<li class='page-item active'><span class='page-link'>{i}</span></li>");
                    }
                    else if(i!=1)
                    {
                        content.AppendLine($"<li class='page-item'><a class='page-link' href='{currentRouteUrl}?pageSize={Model.PageSize}&pageNumber={i}'>{i}</a></li>");
                    }
                }

                if (endPage < totalPages)
                {
                    content.AppendLine("<li class='page-item disabled'><span class='page-link'>...</span></li>");
                }

                // Последняя страница
                if (currentPage < totalPages)
                {
                    content.AppendLine($"<li class='page-item'><a class='page-link' href='{currentRouteUrl}?pageSize={Model.PageSize}&pageNumber={totalPages}'>Last</a></li>");
                }

                content.AppendLine("</ul>");
                output.Content.SetHtmlContent(content.ToString());
                //output.TagName = "div";
                //output.TagMode = TagMode.StartTagAndEndTag;
                //var content = new StringBuilder("");
                //var currentRouteUrl = ViewContext.HttpContext.Request.Path;
                //for (int i = 0; i < Model.TotalPages; i++)
                //{
                //    content.AppendLine($"<a href=\"{currentRouteUrl}?pageSize={Model.PageSize}&pageNumber={i + 1}\">");
                //    content.AppendLine((i + 1).ToString());
                //    content.AppendLine("</a>");
                //    _logger.LogTrace($"Anchor for page {i} has been generated");
                //}

                //output.Content.SetHtmlContent(content.ToString());
            }
        }
    }
}
