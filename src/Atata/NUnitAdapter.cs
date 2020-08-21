using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Atata
{
    internal static class NUnitAdapter
    {
        private const string NUnitAssemblyName = "nunit.framework";

        internal const string AssertionExceptionTypeName = "NUnit.Framework.AssertionException";

        private static readonly Lazy<Type> TestExecutionContextType = new Lazy<Type>(
            () => GetType("NUnit.Framework.Internal.TestExecutionContext"));

        private static readonly Lazy<Type> AssertType = new Lazy<Type>(
            () => GetType("NUnit.Framework.Assert"));

        private static readonly Lazy<Type> TestDelegateType = new Lazy<Type>(
            () => GetType("NUnit.Framework.TestDelegate"));

        private static readonly Lazy<Type> AssertionStatusType = new Lazy<Type>(
            () => GetType("NUnit.Framework.Interfaces.AssertionStatus"));

        private static readonly Lazy<Type> TestResultType = new Lazy<Type>(
            () => GetType("NUnit.Framework.Internal.TestResult"));

        public enum AssertionStatus
        {
            Inconclusive,

            Passed,

            Warning,

            Failed,

            Error
        }

        internal static Type AssertionExceptionType => GetType(AssertionExceptionTypeName);

        private static Type GetType(string typeName)
        {
            return Type.GetType($"{typeName},{NUnitAssemblyName}", true);
        }

        internal static void AssertMultiple(Action action)
        {
            Delegate testDelegate = ConvertToDelegate(action, TestDelegateType.Value);

            MethodInfo assertMultipleMethod = AssertType.Value.GetMethodWithThrowOnError("Multiple", TestDelegateType.Value);

            assertMultipleMethod.InvokeStaticAsLambda(testDelegate);
        }

        private static Delegate ConvertToDelegate(Action action, Type delegateType)
        {
            var targetExpression = Expression.Constant(action.Target);
            var callExpression = Expression.Call(targetExpression, action.Method);

            var lambda = Expression.Lambda(delegateType, callExpression);
            return lambda.Compile();
        }

        internal static void AssertFail(string message)
        {
            var reportFailureMethod = AssertType.Value.GetMethodWithThrowOnError("Fail", typeof(string));

            reportFailureMethod.InvokeStaticAsLambda(message);
        }

        internal static void RecordAssertionIntoTestResult(AssertionStatus status, string message, string stackTrace)
        {
            object testResult = GetCurrentTestResult();

            object statusConverted = Enum.Parse(AssertionStatusType.Value, status.ToString());

            MethodInfo recordAssertionMethod = TestResultType.Value.GetMethodWithThrowOnError("RecordAssertion", AssertionStatusType.Value, typeof(string), typeof(string));
            recordAssertionMethod.InvokeAsLambda(testResult, statusConverted, message, stackTrace);
        }

        internal static void RecordTestCompletionIntoTestResult()
        {
            object testResult = GetCurrentTestResult();

            TestResultType.Value.GetMethodWithThrowOnError("RecordTestCompletion").
                InvokeAsLambda(testResult);
        }

        internal static object GetCurrentTestResult()
        {
            object testExecutionContext = GetCurrentTestExecutionContext();

            return TestExecutionContextType.Value.GetPropertyWithThrowOnError("CurrentResult").
                GetValue(testExecutionContext);
        }

        internal static object GetCurrentTestExecutionContext()
        {
            PropertyInfo currentContextProperty = TestExecutionContextType.Value.GetPropertyWithThrowOnError("CurrentContext");

            return currentContextProperty.GetStaticValue();
        }
    }
}
