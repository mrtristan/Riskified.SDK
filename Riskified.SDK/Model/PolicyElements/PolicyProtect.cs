using System.Collections.Generic;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.PolicyElements
{
    public class PolicyProtect
    {
        [JsonProperty(PropertyName = "use_cases")]
        public List<UseCase> Policies { get; set; }
    }

    public class UseCase
    {
        [JsonProperty(PropertyName = "use_case")]
        public string PolicyType { get; set; }

        [JsonProperty(PropertyName = "decision")]
        public string Decision { get; set; }
    }
}


