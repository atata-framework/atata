namespace Atata;

/// <summary>
/// Provides a method that detects CI environment.
/// </summary>
public static class CIDetector
{
    /// <summary>
    /// Determines whether the code is running inside a Continuous Integration (CI) environment.
    /// Checks the universal "CI=true" variable and common vendor-specific variables:
    /// <c>"TF_BUILD"</c>, <c>"TEAMCITY_VERSION</c>, <c>"JENKINS_HOME"</c>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the code is running on CI; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsRunningOnCI()
    {
        IDictionary variables = Environment.GetEnvironmentVariables();

        return variables["CI"]?.ToString()!.Equals("true", StringComparison.OrdinalIgnoreCase) == true
            || variables.Contains("TF_BUILD")
            || variables.Contains("TEAMCITY_VERSION")
            || variables.Contains("JENKINS_HOME");
    }
}
