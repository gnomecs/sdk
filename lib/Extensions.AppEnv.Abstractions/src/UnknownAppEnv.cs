using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace Gnome.Extensions.Application;

public class UnknownAppEnv : IAppEnv
{
    public static IAppEnv Instance { get; } = new UnknownAppEnv();

    public string Name { get; } = "Unknown";

    public string Version { get; } = "0.0.0";

    public string Id { get; } = "Unknown";

    public string? InstanceName { get; } = null;

    public string EnvironmentName { get; } = "Unknown";

    public IDictionary<string, object?> Properties { get; } = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

    public IFileProvider ContentRootFileProvider { get; } = new NullFileProvider();

    public string ContentRootPath { get; } = Directory.GetCurrentDirectory();

    public object? this[string key]
    {
        get
        {
            if (this.Properties.TryGetValue(key, out var value))
                return value;

            return null;
        }

        set => this.Properties[key] = value;
    }

    public bool IsEnvironment(string environment)
    {
        return environment == "Unknown";
    }
}