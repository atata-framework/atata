using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Atata
{
    internal static class NUnitAdapter
    {
        private const string NUnitAssemblyName = "nunit.framework";

        internal const string AssertionExceptionTypeName = "NUnit.Framework.AssertionException";

        private static readonly Lazy<Type> s_testContextType = new(
            () => GetType("NUnit.Framework.TestContext"));

        private static readonly Lazy<Type> s_testExecutionContextType = new(
            () => GetType("NUnit.Framework.Internal.TestExecutionContext"));

        private static readonly Lazy<Type> s_assertType = new(
            () => GetType("NUnit.Framework.Assert"));

        private static readonly Lazy<Type> s_testDelegateType = new(
            () => GetType("NUnit.Framework.TestDelegate"));

        private static readonly Lazy<Type> s_assertionStatusType = new(
            () => GetType("NUnit.Framework.Interfaces.AssertionStatus"));

        private static readonly Lazy<Type> s_testResultType = new(
            () => GetType("NUnit.Framework.Internal.TestResult"));

        private static readonly Lazy<Type> s_testMethodType = new(
            () => GetType("NUnit.Framework.Internal.TestMethod"));

        private static readonly Lazy<Type> s_testFixtureType = new(
            () => GetType("NUnit.Framework.Internal.TestFixture"));

        private static readonly Lazy<Type> s_setUpFixtureType = new(
            () => GetType("NUnit.Framework.Internal.SetUpFixture"));

        internal enum AssertionStatus
        {
            Inconclusive,

            Passed,

            Warning,

            Failed,

            Error
        }

        internal static Type AssertionExceptionType => GetType(AssertionExceptionTypeName);

        private static Type GetType(string typeName) =>
            Type.GetType($"{typeName},{NUnitAssemblyName}", true);

        internal static object GetCurrentTest()
        {
            object testExecutionContext = GetCurrentTestExecutionContext();

            return s_testExecutionContextType.Value.GetPropertyWithThrowOnError("CurrentTest")
                .GetValue(testExecutionContext);
        }

        internal static string GetCurrentTestName()
        {
            object testItem = GetCurrentTest();

            return testItem != null && testItem.GetType() == s_testMethodType.Value
                ? s_testMethodType.Value.GetPropertyWithThrowOnError("Name").GetValue(testItem) as string
                : null;
        }

        internal static string GetCurrentTestFixtureName()
        {
            dynamic testItem = GetCurrentTest();

            if (testItem.GetType() == s_setUpFixtureType.Value)
                return testItem.TypeInfo.Type.Name;

            do
            {
                if (testItem.GetType() == s_testFixtureType.Value)
                    return testItem.Name;

                testItem = testItem.Parent;
            }
            while (testItem != null);

            return null;
        }

        internal static Type GetCurrentTestFixtureType()
        {
            dynamic testItem = GetCurrentTest();

            if (testItem.GetType() == s_setUpFixtureType.Value)
                return testItem.TypeInfo.Type;

            do
            {
                if (testItem.GetType() == s_testFixtureType.Value)
                    return testItem.TypeInfo.Type;

                testItem = testItem.Parent;
            }
            while (testItem != null);

            return null;
        }

        internal static void AssertMultiple(Action action)
        {
            Delegate testDelegate = ConvertToDelegate(action, s_testDelegateType.Value);

            MethodInfo assertMultipleMethod = s_assertType.Value.GetMethodWithThrowOnError("Multiple", s_testDelegateType.Value);

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
            var reportFailureMethod = s_assertType.Value.GetMethodWithThrowOnError("Fail", typeof(string));

            reportFailureMethod.InvokeStaticAsLambda(message);
        }

        internal static void RecordAssertionIntoTestResult(AssertionStatus status, string message, string stackTrace)
        {
            object testResult = GetCurrentTestResult();

            object statusConverted = Enum.Parse(s_assertionStatusType.Value, status.ToString());

            MethodInfo recordAssertionMethod = s_testResultType.Value.GetMethodWithThrowOnError("RecordAssertion", s_assertionStatusType.Value, typeof(string), typeof(string));
            recordAssertionMethod.InvokeAsLambda(testResult, statusConverted, message, stackTrace);
        }

        internal static void RecordTestCompletionIntoTestResult()
        {
            object testResult = GetCurrentTestResult();

            s_testResultType.Value.GetMethodWithThrowOnError("RecordTestCompletion")
                .InvokeAsLambda(testResult);
        }

        internal static object GetCurrentTestResult()
        {
            object testExecutionContext = GetCurrentTestExecutionContext();

            return s_testExecutionContextType.Value.GetPropertyWithThrowOnError("CurrentResult")
                .GetValue(testExecutionContext);
        }

        internal static object GetCurrentTestExecutionContext()
        {
            PropertyInfo currentContextProperty = s_testExecutionContextType.Value.GetPropertyWithThrowOnError("CurrentContext");

            return currentContextProperty.GetStaticValue();
        }

        internal static dynamic GetCurrentTestResultAdapter()
        {
            dynamic testContext = GetCurrentTestContext();
            return testContext.Result;
        }

        internal static bool IsCurrentTestFailed()
        {
            dynamic testResult = GetCurrentTestResultAdapter();
            return IsTestResultAdapterFailed(testResult);
        }

        internal static bool IsTestResultAdapterFailed(dynamic testResult) =>
            testResult.Outcome.Status.ToString().Contains("Fail");

        // TODO: Change implementation to: TestExecutionContext.CurrentContext.CurrentResult.TestAttachments.Add(new TestAttachment(filePath, description))
        internal static void AddTestAttachment(string filePath, string description = null) =>
            s_testContextType.Value.GetMethodWithThrowOnError("AddTestAttachment", BindingFlags.Static | BindingFlags.Public)
                .InvokeStatic(filePath, description);

        internal static object GetCurrentTestContext()
        {
            PropertyInfo currentContextProperty = s_testContextType.Value.GetPropertyWithThrowOnError("CurrentContext");

            return currentContextProperty.GetStaticValue();
        }
    }
}
