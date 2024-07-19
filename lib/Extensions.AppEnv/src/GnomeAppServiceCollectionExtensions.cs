using System.Diagnostics;

using Gnome.Extensions.Application;

using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class GnomeAppServiceCollectionExtensions
{
    public static IServiceCollection AddGsApplicationPaths(this IServiceCollection services, Action<ApplicationPathsOptions>? configure)
    {
        var options = new ApplicationPathsOptions();
        configure?.Invoke(options);
        services.TryAddSingleton<IAppPaths>(s =>
        {
            if (AppPaths.Current is not UnknownAppPaths)
                return AppPaths.Current;

            var appInfo = s.GetService<IAppEnv>();
            var paths = new AppPaths(options, appInfo);
            AppPaths.Current = paths;
            return paths;
        });

        return services;
    }

    public static IServiceCollection AddGsApplicationEnvironment(
        this IServiceCollection services,
        Action<AppEnvOptions>? configure,
        Func<IServiceProvider, MicrosoftHostEnvironment>? configureMicrosoftHostEnvironment,
        Type? hostingEnvironmentType = null)
    {
        var options = new AppEnvOptions();
        configure?.Invoke(options);

        if (configureMicrosoftHostEnvironment is null)
        {
            configureMicrosoftHostEnvironment = s =>
            {
                var microsoftHostEnvironment = new MicrosoftHostEnvironment();
                Type? hostEnvironmentType = null;
                try
                {
                    if (hostingEnvironmentType is null)
                    {
#pragma warning disable IL2096 // trimming can not guarantee the type exists
                        hostEnvironmentType = Type.GetType("Microsoft.Extensions.Hosting.IHostingEnvironment", false, true);
#pragma warning restore IL2096
                    }

                    if (hostEnvironmentType is not null)
                    {
                        var hostEnvironment = s.GetService(hostEnvironmentType);
                        if (hostEnvironment is not null)
                        {
                            var applicationName = hostEnvironmentType.GetProperty("ApplicationName")
                                ?.GetValue(hostEnvironmentType) as string;
                            var environmentName = hostEnvironmentType.GetProperty("EnvironmentName")
                                ?.GetValue(hostEnvironmentType) as string;
                            var contentRootPath = hostEnvironmentType.GetProperty("ContentRootPath")
                                ?.GetValue(hostEnvironmentType) as string;
                            var contentRootFileProvider =
                                hostEnvironmentType.GetProperty("ContentRootFileProvider")
                                    ?.GetValue(hostEnvironmentType) as IFileProvider;

                            microsoftHostEnvironment.EnvironmentName ??= environmentName;
                            microsoftHostEnvironment.ApplicationName ??= applicationName;
                            microsoftHostEnvironment.ContentRootPath ??= contentRootPath;
                            microsoftHostEnvironment.ContentRootFileProvider ??= contentRootFileProvider;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                return microsoftHostEnvironment;
            };
        }

        services.TryAddSingleton<IAppEnv>(s =>
        {
            if (AppEnv.Current is not UnknownAppEnv)
                return AppEnv.Current;

            var environment = configureMicrosoftHostEnvironment(s);
            var env = new AppEnv(options, environment);
            AppEnv.Current = env;
            return env;
        });

        return services;
    }
}