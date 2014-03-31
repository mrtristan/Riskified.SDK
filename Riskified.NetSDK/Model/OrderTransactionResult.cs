﻿using System.Net.Cache;
using Newtonsoft.Json;

namespace Riskified.NetSDK.Model
{
    public class OrderTransactionResult
    {
        [JsonProperty(PropertyName = "order",Required = Required.Default)]
        public SuccessfulOrderTransactionData SuccessfulResult { get; set; }

        [JsonProperty(PropertyName = "error", Required = Required.Default)]
        public FailedTransactionData FailedResult { get; set; }


        /// <summary>
        /// A flag that signs if the transaction was finished successfully
        /// Values of SuccessfulResult and FailedResult will be set accordingly (one will be null)
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessful
        {
            get { return SuccessfulResult != null; }
        }
    }

    public class FailedTransactionData
    {
        [JsonProperty(PropertyName = "message",Required = Required.Always)]
        public string ErrorMessage { get; set; }
    }

    public class SuccessfulOrderTransactionData
    {

        [JsonProperty(PropertyName = "id",Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "status", Required = Required.Always)]
        public string Status { get; set; }
    }
}
