namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple=false, Inherited=false), Conditional("CONTRACTS_FULL")]
    internal sealed class MutabilityAttribute : Attribute
    {
        private System.Diagnostics.Contracts.Mutability _mutabilityMarker;

        public MutabilityAttribute(System.Diagnostics.Contracts.Mutability mutabilityMarker)
        {
            this._mutabilityMarker = mutabilityMarker;
        }

        public System.Diagnostics.Contracts.Mutability Mutability
        {
            get
            {
                return this._mutabilityMarker;
            }
        }
    }
}

