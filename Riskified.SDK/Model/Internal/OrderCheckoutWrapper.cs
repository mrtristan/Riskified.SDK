using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    internal class OrderCheckoutWrapper<TOrderCheckout>
    {
        [JsonProperty(PropertyName = "checkout", Required = Required.Always)]
        public TOrderCheckout Order { get; set; }

        [JsonProperty(PropertyName = "warnings")]
        public string[] Warnings { get; set; }

        public OrderCheckoutWrapper(TOrderCheckout order)
        {
            Order = order;
        }
    }
}
