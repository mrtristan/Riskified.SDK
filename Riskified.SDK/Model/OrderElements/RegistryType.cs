using System.Runtime.Serialization;

namespace Riskified.SDK.Model.OrderElements
{
    public enum RegistryType
    {
        [EnumMember(Value = "wedding")]
        Wedding,
        [EnumMember(Value = "baby")]
        Baby,
        [EnumMember(Value = "other")]
        Other
    }
}
