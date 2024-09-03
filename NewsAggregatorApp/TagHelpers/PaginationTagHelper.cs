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
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                var content = new StringBuilder("");
                var currentRouteUrl = ViewContext.HttpContext.Request.Path;
                for (int i = 0; i < Model.TotalPages; i++)
                {
                    content.AppendLine($"<a href=\"{currentRouteUrl}?pageSize={Model.PageSize}&pageNumber={i + 1}\">");
                    content.AppendLine((i + 1).ToString());
                    content.AppendLine("</a>");
                    _logger.LogTrace($"Anchor for page {i} has been generated");
                }

                output.Content.SetHtmlContent(content.ToString());
            }
        }
    }
}
