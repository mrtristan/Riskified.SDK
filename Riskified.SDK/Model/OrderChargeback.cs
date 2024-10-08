﻿using Newtonsoft.Json;
using Riskified.SDK.Model.OrderElements;
using Riskified.SDK.Utils;
using System.Collections.Generic;
using Riskified.SDK.Model.ChargebackElements;

namespace Riskified.SDK.Model
{
    public class OrderChargeback : OrderBase
    {
        /// <summary>
        /// Creates a new order chargeback
        /// </summary>
        /// <param name="merchantOrderId">The unique id of the order at the merchant systems</param>
        public OrderChargeback(string merchantOrderId, ChargebackDetails chargebackDetails, List <FulfillmentDetails> fulfillment, DisputeDetails disputeDetails)
            : base(merchantOrderId)
        {
            this.Chargeback = chargebackDetails;
            this.Fulfillments = fulfillment;
            this.Dispute = disputeDetails;
        }

        public override void Validate(Validations validationType = Validations.Weak)
        {
            base.Validate(validationType);
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "chargeback_details")]
        public ChargebackDetails Chargeback { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "fulfillments")]
        public List<FulfillmentDetails> Fulfillments { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "dispute_details")]
        public DisputeDetails Dispute { get; set; }

    }
}
