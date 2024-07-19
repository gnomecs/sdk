using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.FileProviders;

namespace Gnome.Extensions.Application;

public interface IAppEnv
{
    string Name { get; }

    string Version { get; }

    string Id { get; }

    string? InstanceName { get; }

    string EnvironmentName { get; }

    IFileProvider ContentRootFileProvider { get; }

    string ContentRootPath { get; }

    IDictionary<string, object?> Properties { get; }

    object? this[string key] { get; set; }

    bool IsEnvironment(string environment);
}