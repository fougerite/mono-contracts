namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [Conditional("CONTRACTS_FULL"), AttributeUsage(AttributeTargets.All, AllowMultiple=true, Inherited=false)]
    public sealed class ContractVerificationAttribute : Attribute
    {
        private string _value;

        public ContractVerificationAttribute(bool staticChecking)
        {
        }

        public ContractVerificationAttribute(string value)
        {
            this._value = value;
        }

        public string Value
        {
            get
            {
                return this._value;
            }
        }
    }
}

