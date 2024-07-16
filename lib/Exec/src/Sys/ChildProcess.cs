using Gnome.Exec;

namespace Gnome.Sys;

public static class ChildProcess
{
    public static PsPipe Chain(PsStartInfo startInfo)
        => new(startInfo);

    public static PsPipe Chain(string fileName)
        => new(new PsStartInfo(fileName));

    public static PsPipe Chain(string fileName, PsArgs args)
        => new(new PsStartInfo(fileName, args));

    public static PsPipe Chain(IPsStartInfoBuilder builder)
        => new(builder.Build());

    public static PsPipe Chain(PsChild child)
        => new(child);

    public static PsPipeAsync ChainAsync(PsStartInfo startInfo)
        => new(startInfo);

    public static PsPipeAsync ChainAsync(string fileName)
        => new(new PsStartInfo(fileName));

    public static PsPipeAsync ChainAsync(string fileName, PsArgs args)
        => new(new PsStartInfo(fileName, args));

    public static PsPipeAsync ChainAsync(IPsStartInfoBuilder builder)
        => new(builder.Build());

    public static PsPipeAsync ChainAsync(PsChild child)
        => new(child);

    public static PsOutput Output(PsStartInfo startInfo)
    {
        using var child = new PsChild(startInfo);
        var output = child.WaitForOutput();
        return output;
    }

    public static PsOutput Output(string fileName)
    {
        using var child = new PsChild(new PsStartInfo(fileName));
        var output = child.WaitForOutput();
        return output;
    }

    public static PsOutput Output(string fileName, PsArgs args)
    {
        using var child = new PsChild(new PsStartInfo(fileName, args));
        var output = child.WaitForOutput();
        return output;
    }

    public static PsOutput Output(IPsStartInfoBuilder builder)
    {
        using var child = new PsChild(builder.Build());
        var output = child.WaitForOutput();
        return output;
    }

    public static async Task<PsOutput> OutputAsync(PsStartInfo startInfo, CancellationToken cancellationToken = default)
    {
        using var child = new PsChild(startInfo);
        var output = await child.WaitForOutputAsync(cancellationToken);
        return output;
    }

    public static async Task<PsOutput> OutputAsync(string fileName, CancellationToken cancellationToken = default)
    {
        using var child = new PsChild(new PsStartInfo(fileName));
        var output = await child.WaitForOutputAsync(cancellationToken);
        return output;
    }

    public static async Task<PsOutput> OutputAsync(string fileName, PsArgs args, CancellationToken cancellationToken = default)
    {
        using var child = new PsChild(new PsStartInfo(fileName, args));
        var output = await child.WaitForOutputAsync(cancellationToken);
        return output;
    }

    public static async Task<PsOutput> OutputAsync(IPsStartInfoBuilder builder, CancellationToken cancellationToken = default)
    {
        using var child = new PsChild(builder.Build());
        var output = await child.WaitForOutputAsync(cancellationToken);
        return output;
    }

    public static PsChild Spawn(PsStartInfo startInfo)
        => new(startInfo);

    public static PsChild Spawn(string fileName)
        => new(new PsStartInfo(fileName));

    public static PsChild Spawn(string fileName, PsArgs args)
        => new(new PsStartInfo(fileName, args));

    public static PsChild Spawn(IPsStartInfoBuilder builder)
        => new(builder.Build());

    public static PsOutput Run(string fileName, PsArgs args)
    {
        var command = new Command(fileName)
            .WithArgs(args);

        return command.Run();
    }

    public static PsOutput Run(string fileName)
    {
        var command = new Command(fileName);
        return command.Run();
    }

    public static PsOutput Run(PsStartInfo startInfo)
    {
        var command = new Command(startInfo);
        return command.Run();
    }

    public static PsOutput Run(IPsStartInfoBuilder builder)
        => Run(builder.Build());

    public static async Task<PsOutput> RunAsync(string fileName, PsArgs args, CancellationToken cancellationToken = default)
    {
        var command = new Command(fileName)
            .WithArgs(args);

        return await command.RunAsync(cancellationToken);
    }

    public static async Task<PsOutput> RunAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var command = new Command(fileName);
        return await command.RunAsync(cancellationToken);
    }

    public static async Task<PsOutput> RunAsync(PsStartInfo startInfo, CancellationToken cancellationToken = default)
    {
        var command = new Command(startInfo);
        return await command.RunAsync(cancellationToken);
    }

    public static Task<PsOutput> RunAsync(IPsStartInfoBuilder builder, CancellationToken cancellationToken = default)
        => RunAsync(builder.Build(), cancellationToken);

    public static string? Which(string fileName, IEnumerable<string>? prependPaths = null, bool useCache = true)
        => PathFinder.Which(fileName, prependPaths, useCache);
}