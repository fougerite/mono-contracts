namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false), Conditional("CONTRACTS_FULL")]
    public sealed class ContractClassForAttribute : Attribute
    {
        private Type _typeIAmAContractFor;

        public ContractClassForAttribute(Type typeContractsAreFor)
        {
            this._typeIAmAContractFor = typeContractsAreFor;
        }

        public Type TypeContractsAreFor
        {
            get
            {
                return this._typeIAmAContractFor;
            }
        }
    }
}

