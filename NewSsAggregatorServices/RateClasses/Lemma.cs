using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorServices.RateClasses
{
    public class Lemma
    {
        [JsonProperty("start")]
        public int Start;

        [JsonProperty("end")]
        public int End;

        [JsonProperty("value")]
        public string Value;
    }
}
