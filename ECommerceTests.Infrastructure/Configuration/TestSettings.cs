using System;

namespace ECommerceTests.Infrastructure.Configuration;

public static class TestSettings
{
    public static readonly TimeSpan ExplicitWaitTimeout = TimeSpan.FromSeconds(20);
    public static readonly TimeSpan PageReadyTimeout = TimeSpan.FromSeconds(20);
    public static readonly TimeSpan PageLoadTimeout = TimeSpan.FromSeconds(60);
    public static readonly TimeSpan DriverCommandTimeout = TimeSpan.FromSeconds(120);

    public static bool RunHeadless =>
        bool.TryParse(Environment.GetEnvironmentVariable("AE_HEADLESS"), out var value) && value;

    public static bool HasExistingLoginCredentials =>
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AE_EMAIL")) &&
        !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("AE_PASSWORD"));

    public static string ExistingUserEmail => GetRequiredEnvironmentVariable("AE_EMAIL");

    public static string ExistingUserPassword => GetRequiredEnvironmentVariable("AE_PASSWORD");

    public static string ExistingUserDisplayName =>
        Environment.GetEnvironmentVariable("AE_USERNAME")?.Trim() ?? string.Empty;

    public static string GetRequiredEnvironmentVariable(string variableName)
    {
        var value = Environment.GetEnvironmentVariable(variableName)?.Trim();

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(
                $"Environment variable '{variableName}' must be configured before running this test.");
        }

        return value;
    }
}
