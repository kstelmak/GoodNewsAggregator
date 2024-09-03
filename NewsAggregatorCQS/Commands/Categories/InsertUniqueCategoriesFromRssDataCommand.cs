using MediatR;
using NewsAggregatorDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorCQS.Commands.Categories
{
    public class InsertUniqueCategoriesFromRssDataCommand : IRequest
    {
        //public CategoryDto[] Categories { get; set; }
        public string[] CategoriesNames { get; set; }
    }
}
