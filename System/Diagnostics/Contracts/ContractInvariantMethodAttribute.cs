namespace System.Diagnostics.Contracts
{
    using System;
    using System.Diagnostics;

    [Conditional("CONTRACTS_FULL"), AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public sealed class ContractInvariantMethodAttribute : Attribute
    {
    }
}

