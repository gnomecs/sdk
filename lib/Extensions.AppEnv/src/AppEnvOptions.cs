using System.Reflection;

using Microsoft.Extensions.FileProviders;

namespace Gnome.Extensions.Application;

public class AppEnvOptions
{
    public string? Name { get; set; }

    public string? Id { get; set; }

    public string? Version { get; set; }

    public string? InstanceName { get; set; }

    public Assembly? EntryAssembly { get; set; }

    public string? EnvironmentName { get; set; }

    public bool UseEnvironmentVariables { get; set; } = true;

    public string? ContentRootPath { get; set; }

    public IFileProvider? ContentRootFileProvider { get; set; }

    public Dictionary<string, object?> Properties { get; } = new();
}