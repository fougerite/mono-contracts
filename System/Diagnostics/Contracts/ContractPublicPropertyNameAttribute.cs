namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Field), Conditional("CONTRACTS_FULL")]
    public sealed class ContractPublicPropertyNameAttribute : Attribute
    {
        private string _publicName;

        public ContractPublicPropertyNameAttribute(string name)
        {
            this._publicName = name;
        }

        public string Name
        {
            get
            {
                return this._publicName;
            }
        }
    }
}

