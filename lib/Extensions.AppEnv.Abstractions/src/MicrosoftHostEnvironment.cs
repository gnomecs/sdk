using Microsoft.Extensions.FileProviders;

namespace Gnome.Extensions.Application;

public class MicrosoftHostEnvironment
{
    public string? ApplicationName { get; set; }

    public IFileProvider? ContentRootFileProvider { get; set; }

    public string? ContentRootPath { get; set; }

    public string? EnvironmentName { get; set; }
}