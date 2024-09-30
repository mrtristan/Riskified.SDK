using System.Runtime.Serialization;

namespace Riskified.SDK.Model
{
    public enum TransportMethodType
    {
        [EnumMember(Value = "plane")]
        Plane,
        [EnumMember(Value = "ship")]
        Ship,
        [EnumMember(Value = "bus")]
        Bus,
        [EnumMember(Value = "train")]
        Train
    }
}
