using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;

namespace Riskified.SDK.Model.Internal
{
    internal class Notification
    {
        [JsonProperty(PropertyName = "id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "oldStatus", Required = Required.Default)]
        public string OldStatus { get; set; }

        [JsonProperty(PropertyName = "description", Required = Required.Default)]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "custom", Required = Required.Default)]
        public Custom Custom { get; set; }

        [JsonProperty(PropertyName = "category", Required = Required.Default)]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "decision_code", Required = Required.Default)]
        public string DecisionCode { get; set; }

    }
}
