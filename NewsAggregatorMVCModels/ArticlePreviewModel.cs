using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMVCModels
{
	public class ArticlePreviewModel
	{
		public Guid ArticlePreviewModelId { get; set; }
        public string Title { get; set; }
		public string? Description { get; set; }
		public DateTime PublicationDate { get; set; }
		public double? Rate { get; set; }
		public string? SourceName { get; set; }
	}
}
