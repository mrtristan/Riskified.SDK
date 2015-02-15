﻿using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Riskified.SDK.Model
{
    public class OrderFulfillment : AbstractOrder
    {
        public OrderFulfillment(int merchantOrderId, FulfillmentDetails[] fulfillments)
            : base(merchantOrderId)
        {
            this.Fulfillments = fulfillments;
        }

        public override void Validate(bool isWeak = false)
        {
            base.Validate(isWeak);
            InputValidators.ValidateObjectNotNull(Fulfillments, "Fulfillments");
            Fulfillments.ToList().ForEach(item => item.Validate(isWeak));

        }

        /// <summary>
        /// A list of fulfillment attempts for the order.
        /// </summary>
        [JsonProperty(PropertyName = "fulfillments")]
        public FulfillmentDetails[] Fulfillments { get; set; }
    }
}
