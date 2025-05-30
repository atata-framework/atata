namespace Atata.Xunit;

internal static class TestResultStateExtensions
{
    internal static (string Message, string? StackTrace) ExtractExceptionDetails(this TestResultState testResultState)
    {
        if (testResultState.ExceptionTypes?.Length > 1
            && testResultState.ExceptionTypes?.Length == testResultState.ExceptionMessages?.Length
            && testResultState.ExceptionTypes?.Length == testResultState.ExceptionStackTraces?.Length)
        {
            StringBuilder messageBuilder = new();

            for (int i = 0; i < testResultState.ExceptionTypes!.Length; i++)
            {
                if (i > 0)
                {
                    messageBuilder.AppendLine()
                        .Append(" ---> ");
                }

                messageBuilder.Append(testResultState.ExceptionTypes[i])
                    .Append(": ")
                    .Append(testResultState.ExceptionMessages![i]);
            }

            StringBuilder stackTraceBuilder = new();

            for (int i = testResultState.ExceptionStackTraces!.Length - 1; i >= 0; i--)
            {
                stackTraceBuilder.Append(testResultState.ExceptionStackTraces![i]);

                if (i > 0)
                {
                    stackTraceBuilder.AppendLine()
                        .Append("   --- End of inner exception stack trace ---")
                        .AppendLine();
                }
            }

            return (messageBuilder.ToString(), stackTraceBuilder.ToString());
        }
        else
        {
            return ($"{testResultState.ExceptionTypes![0]}: {testResultState.ExceptionMessages?[0]}",
                    testResultState.ExceptionStackTraces?[0]);
        }
    }
}
