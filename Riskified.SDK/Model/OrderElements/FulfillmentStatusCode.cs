using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum FulfillmentStatusCode
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "cancelled")]
        Cancelled,
        [EnumMember(Value = "error")]
        Error,
        [EnumMember(Value = "failure")]
        Failure
    }
}
