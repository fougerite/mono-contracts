namespace System.Diagnostics.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts.Internal;
    using System.Runtime.ConstrainedExecution;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    public static class Contract
    {
        public static  event EventHandler<ContractFailedEventArgs> ContractFailed
        {
            [SecurityCritical, SecurityPermission(SecurityAction.LinkDemand, Unrestricted=true)] add
            {
                ContractHelper.InternalContractFailed += value;
            }
            [SecurityCritical, SecurityPermission(SecurityAction.LinkDemand, Unrestricted=true)] remove
            {
                ContractHelper.InternalContractFailed -= value;
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure, Conditional("CONTRACTS_FULL"), Conditional("DEBUG")]
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                ReportFailure(ContractFailureKind.Assert, null, null, null);
            }
        }

        [Conditional("DEBUG"), Pure, Conditional("CONTRACTS_FULL"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Assert(bool condition, string userMessage)
        {
            if (!condition)
            {
                ReportFailure(ContractFailureKind.Assert, userMessage, null, null);
            }
        }

        private static void AssertMustUseRewriter(ContractFailureKind kind, string contractKind)
        {
            ContractHelper.TriggerFailure(kind, "Must use the rewriter when using Contract." + contractKind, null, null, null);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure, Conditional("DEBUG"), Conditional("CONTRACTS_FULL")]
        public static void Assume(bool condition)
        {
            if (!condition)
            {
                ReportFailure(ContractFailureKind.Assume, null, null, null);
            }
        }

        [Conditional("DEBUG"), Conditional("CONTRACTS_FULL"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure]
        public static void Assume(bool condition, string userMessage)
        {
            if (!condition)
            {
                ReportFailure(ContractFailureKind.Assume, userMessage, null, null);
            }
        }

        [Conditional("CONTRACTS_FULL"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static void EndContractBlock()
        {
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure, Conditional("CONTRACTS_FULL")]
        public static void Ensures(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
        }

        [Conditional("CONTRACTS_FULL"), Pure, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Ensures(bool condition, string userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Postcondition, "Ensures");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Conditional("CONTRACTS_FULL"), Pure]
        public static void EnsuresOnThrow<TException>(bool condition) where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
        }

        [Conditional("CONTRACTS_FULL"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure]
        public static void EnsuresOnThrow<TException>(bool condition, string userMessage) where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.PostconditionOnException, "EnsuresOnThrow");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure]
        public static bool Exists<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EndContractBlock();
            foreach (T local in collection)
            {
                if (predicate(local))
                {
                    return true;
                }
            }
            return false;
        }

        [Pure, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static bool Exists(int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (fromInclusive > toExclusive)
            {
                throw new ArgumentException("fromInclusive must be less than or equal to toExclusive");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EndContractBlock();
            for (int i = fromInclusive; i < toExclusive; i++)
            {
                if (predicate(i))
                {
                    return true;
                }
            }
            return false;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure]
        public static bool ForAll<T>(IEnumerable<T> collection, Predicate<T> predicate)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EndContractBlock();
            foreach (T local in collection)
            {
                if (!predicate(local))
                {
                    return false;
                }
            }
            return true;
        }

        [Pure, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static bool ForAll(int fromInclusive, int toExclusive, Predicate<int> predicate)
        {
            if (fromInclusive > toExclusive)
            {
                throw new ArgumentException("fromInclusive must be less than or equal to toExclusive");
            }
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            EndContractBlock();
            for (int i = fromInclusive; i < toExclusive; i++)
            {
                if (!predicate(i))
                {
                    return false;
                }
            }
            return true;
        }

        [Pure, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Conditional("CONTRACTS_FULL")]
        public static void Invariant(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure, Conditional("CONTRACTS_FULL")]
        public static void Invariant(bool condition, string userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Invariant, "Invariant");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), Pure]
        public static T OldValue<T>(T value)
        {
            return default(T);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), DebuggerNonUserCode, SecuritySafeCritical]
        private static void ReportFailure(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
        {
            if ((failureKind < ContractFailureKind.Precondition) || (failureKind > ContractFailureKind.Assume))
            {
                throw new ArgumentException("failureKind is not in range", "failureKind");
            }
            string displayMessage = ContractHelper.RaiseContractFailedEvent(failureKind, userMessage, conditionText, innerException);
            if (displayMessage != null)
            {
                ContractHelper.TriggerFailure(failureKind, displayMessage, userMessage, conditionText, innerException);
            }
        }

        [Conditional("CONTRACTS_FULL"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure]
        public static void Requires(bool condition)
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Pure]
        public static void Requires<TException>(bool condition) where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
        }

        [Pure, Conditional("CONTRACTS_FULL"), ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Requires(bool condition, string userMessage)
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires");
        }

        [Pure, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        public static void Requires<TException>(bool condition, string userMessage) where TException: Exception
        {
            AssertMustUseRewriter(ContractFailureKind.Precondition, "Requires<TException>");
        }

        [DebuggerNonUserCode, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Obsolete("use Requires and the appropriate runtime checking level, or use if-then-throw.")]
        public static void RequiresAlways(bool condition)
        {
            if (!condition)
            {
                string failure = ContractHelper.RaiseContractFailedEvent(ContractFailureKind.Precondition, null, null, null);
                if (failure != null)
                {
                    throw new ContractException(ContractFailureKind.Precondition, failure, null, null, null);
                }
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), Obsolete("use Requires and the appropriate runtime checking level, or use if-then-throw."), DebuggerNonUserCode]
        public static void RequiresAlways(bool condition, string userMessage)
        {
            if (!condition)
            {
                string failure = ContractHelper.RaiseContractFailedEvent(ContractFailureKind.Precondition, userMessage, null, null);
                if (failure != null)
                {
                    throw new ContractException(ContractFailureKind.Precondition, failure, userMessage, null, null);
                }
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), Pure]
        public static T Result<T>()
        {
            return default(T);
        }

        [Pure, ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static T ValueAtReturn<T>(out T value)
        {
            value = default(T);
            return value;
        }
    }
}

