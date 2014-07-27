namespace System.Diagnostics.Contracts.Internal
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.ConstrainedExecution;
    using System.Security;

    public static class ContractHelper
    {
        private static EventHandler<ContractFailedEventArgs> contractFailedEvent;
        private static readonly object lockObject = new object();

        internal static  event EventHandler<ContractFailedEventArgs> InternalContractFailed
        {
            [SecurityCritical] add
            {
                RuntimeHelpers.PrepareDelegate(value);
                lock (lockObject)
                {
                    contractFailedEvent = (EventHandler<ContractFailedEventArgs>) Delegate.Combine(contractFailedEvent, value);
                }
            }
            [SecurityCritical] remove
            {
                lock (lockObject)
                {
                    contractFailedEvent = (EventHandler<ContractFailedEventArgs>) Delegate.Remove(contractFailedEvent, value);
                }
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private static string GetDisplayMessage(ContractFailureKind failureKind, string userMessage, string conditionText)
        {
            string str2 = null;
            switch (failureKind)
            {
                case ContractFailureKind.Precondition:
                    str2 = Properties.Resources.PreconditionFailed;
                    break;

                case ContractFailureKind.Postcondition:
                    str2 = Properties.Resources.PostconditionFailed;
                    break;

                case ContractFailureKind.PostconditionOnException:
                    str2 = Properties.Resources.PostconditionOnExceptionFailed;
                    break;

                case ContractFailureKind.Invariant:
                    str2 = Properties.Resources.InvariantFailed;
                    break;

                case ContractFailureKind.Assert:
                    str2 = Properties.Resources.AssertionFailed;
                    break;

                case ContractFailureKind.Assume:
                    str2 = Properties.Resources.AssumptionFailed;
                    break;

                default:
                    Contract.Assume(false, "Unreachable code");
                    str2 = Properties.Resources.AssumptionFailed;
                    break;
            }
            if (str2 == null)
            {
                str2 = string.Format(CultureInfo.CurrentUICulture, "{0} failed", new object[] { failureKind });
            }
            if (!string.IsNullOrEmpty(conditionText))
            {
                if (!string.IsNullOrEmpty(userMessage))
                {
                    return string.Format(CultureInfo.CurrentUICulture, "{0}: {1} {2}", new object[] { str2, conditionText, userMessage });
                }
                return string.Format(CultureInfo.CurrentUICulture, "{0}: {1}", new object[] { str2, conditionText });
            }
            if (!string.IsNullOrEmpty(userMessage))
            {
                return string.Format(CultureInfo.CurrentUICulture, "{0}: {1}", new object[] { str2, userMessage });
            }
            return str2;
        }

        [DebuggerNonUserCode, ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail), SecuritySafeCritical]
        public static string RaiseContractFailedEvent(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException)
        {
            string resultFailureMessage = "Contract failed";
            RaiseContractFailedEventImplementation(failureKind, userMessage, conditionText, innerException, ref resultFailureMessage);
            return resultFailureMessage;
        }

        [DebuggerNonUserCode]
        private static void RaiseContractFailedEventImplementation(ContractFailureKind failureKind, string userMessage, string conditionText, Exception innerException, ref string resultFailureMessage)
        {
            string str2;
            if ((failureKind < ContractFailureKind.Precondition) || (failureKind > ContractFailureKind.Assume))
            {
                throw new ArgumentException("failureKind is not in range", "failureKind");
            }
            string message = "contract failed.";
            ContractFailedEventArgs e = null;
            try
            {
                message = GetDisplayMessage(failureKind, userMessage, conditionText);
                if (contractFailedEvent != null)
                {
                    e = new ContractFailedEventArgs(failureKind, message, conditionText, innerException);
                    foreach (EventHandler<ContractFailedEventArgs> handler in contractFailedEvent.GetInvocationList())
                    {
                        try
                        {
                            handler(null, e);
                        }
                        catch (Exception exception)
                        {
                            e.thrownDuringHandler = exception;
                            e.SetUnwindNoDemand();
                        }
                    }
                    if (e.Unwind)
                    {
                        if (innerException == null)
                        {
                            innerException = e.thrownDuringHandler;
                        }
                        throw new ContractException(failureKind, message, userMessage, conditionText, innerException);
                    }
                }
            }
            finally
            {
                if ((e != null) && e.Handled)
                {
                    str2 = null;
                }
                else
                {
                    str2 = message;
                }
            }
            resultFailureMessage = str2;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), SecuritySafeCritical, DebuggerNonUserCode]
        public static void TriggerFailure(ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
        {
            TriggerFailureImplementation(kind, displayMessage, userMessage, conditionText, innerException);
        }

        [DebuggerNonUserCode]
        private static void TriggerFailureImplementation(ContractFailureKind kind, string displayMessage, string userMessage, string conditionText, Exception innerException)
        {
            if (!Environment.UserInteractive)
            {
                Environment.FailFast(displayMessage);
            }
            Debug.Assert(false, displayMessage);
        }
    }
}

