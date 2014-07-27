namespace System.Diagnostics.Contracts
{
    using System;

    [Serializable]
    internal enum Mutability
    {
        Immutable,
        Mutable,
        HasInitializationPhase
    }
}

