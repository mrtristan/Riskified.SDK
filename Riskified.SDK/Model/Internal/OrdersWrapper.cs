using System.Collections.Generic;
using Newtonsoft.Json;

namespace Riskified.SDK.Model.Internal
{
    internal class OrdersWrapper
    {
        [JsonProperty(PropertyName = "orders")]
        public IEnumerable<AbstractOrder> Orders { get; set; }

        public OrdersWrapper(IEnumerable<AbstractOrder> orders)
        {
            Orders = orders;
        }
    }
}
