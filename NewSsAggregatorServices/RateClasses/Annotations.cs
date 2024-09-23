using NewsAggregatorApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorServices.RateClasses
{
    public class Annotations
    {
        [JsonProperty("lemma")]
        public List<Lemma> Lemmas;
    }
}
