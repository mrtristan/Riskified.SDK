﻿using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    // Used on the line item level, the pickup location for that product
    public enum DeliveredToType
    {
        [EnumMember(Value = "shipping_address")]
        ShippingAddress,
        [EnumMember(Value = "store_pickup")]
        StorePickup
    }
}
