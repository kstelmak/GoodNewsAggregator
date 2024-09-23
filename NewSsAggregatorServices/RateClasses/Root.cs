using NewsAggregatorApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorServices.RateClasses
{
    public class Root
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("annotations")]
        public Annotations Annotations { get; set; }
    }
}
