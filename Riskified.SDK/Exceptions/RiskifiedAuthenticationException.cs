using System;

namespace Riskified.SDK.Exceptions
{
    public class RiskifiedAuthenticationException : RiskifiedException
    {
        public RiskifiedAuthenticationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public RiskifiedAuthenticationException(string message)
            : base(message)
        {
        }
    }
}
