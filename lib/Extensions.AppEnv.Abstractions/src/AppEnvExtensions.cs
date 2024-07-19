namespace Gnome.Extensions.Application;

public static class AppEnvExtensions
{
    public static bool IsProduction(this IAppEnv applicationEnvironment)
    {
        if (!applicationEnvironment.Properties.TryGetValue("ProductionEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationEnvironment.IsEnvironment("production");
        }

        foreach (var test in tests)
        {
            if (applicationEnvironment.IsEnvironment(test))
                return true;
        }

        return applicationEnvironment.IsEnvironment("production");
    }

    public static bool IsDevelopment(this IAppEnv applicationEnvironment)
    {
        if (!applicationEnvironment.Properties.TryGetValue("DevelopmentEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationEnvironment.IsEnvironment("development");
        }

        foreach (var test in tests)
        {
            if (applicationEnvironment.IsEnvironment(test))
                return true;
        }

        return applicationEnvironment.IsEnvironment("development");
    }

    public static bool IsTest(this IAppEnv applicationEnvironment)
    {
        if (!applicationEnvironment.Properties.TryGetValue("TestEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationEnvironment.IsEnvironment("test");
        }

        foreach (var test in tests)
        {
            if (applicationEnvironment.IsEnvironment(test))
                return true;
        }

        return applicationEnvironment.IsEnvironment("test");
    }

    public static bool IsTestHost(this IAppEnv applicationEnvironment)
    {
        if (applicationEnvironment.Properties.TryGetValue("TestHost", out var isTestHostValue) && isTestHostValue is bool isTestHost)
            return isTestHost;

        if (!applicationEnvironment.Properties.TryGetValue("TestHostEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationEnvironment.IsEnvironment("testhost");
        }

        foreach (var test in tests)
        {
            if (applicationEnvironment.IsEnvironment(test))
                return true;
        }

        return applicationEnvironment.IsEnvironment("testhost");
    }

    public static bool IsStaging(this IAppEnv applicationEnvironment)
    {
        if (!applicationEnvironment.Properties.TryGetValue("StagingEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationEnvironment.IsEnvironment("staging");
        }

        foreach (var test in tests)
        {
            if (applicationEnvironment.IsEnvironment(test))
                return true;
        }

        return applicationEnvironment.IsEnvironment("staging");
    }

    public static bool IsQualityAssurance(this IAppEnv applicationEnvironment)
    {
        if (!applicationEnvironment.Properties.TryGetValue("QaEnvNames", out var productionTests) ||
            productionTests is not IEnumerable<string> tests)
        {
            return applicationEnvironment.IsEnvironment("qa");
        }

        foreach (var test in tests)
        {
            if (applicationEnvironment.IsEnvironment(test))
                return true;
        }

        return applicationEnvironment.IsEnvironment("qa");
    }
}