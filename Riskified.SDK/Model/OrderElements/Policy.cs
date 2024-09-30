using Newtonsoft.Json;

namespace Riskified.SDK.Model.OrderElements
{
    public class Policy
    {
        public Policy(bool? evaluate)
        {
            Evaluate = evaluate;
        }

        [JsonProperty(PropertyName = "evaluate")]
        public bool? Evaluate { get; set; }
    }
}
