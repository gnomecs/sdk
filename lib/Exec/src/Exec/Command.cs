using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;

using Gnome.Sys;
using Gnome.Sys.IO;

namespace Gnome.Exec;

public class Command : IPsStartInfoBuilder
{
    private readonly PsStartInfo startInfo;

    public Command(string fileName)
    {
        this.startInfo = new PsStartInfo(fileName);
    }

    public Command(string fileName, PsArgs args)
    {
        this.startInfo = new PsStartInfo(fileName, args);
    }

    public Command(PsStartInfo startInfo)
    {
        this.startInfo = startInfo;
    }

    public Command WithArgs(PsArgs args)
    {
        this.startInfo.Args = args;
        return this;
    }

    public Command WithCwd(string cwd)
    {
        this.startInfo.Cwd = cwd;
        return this;
    }

    public Command WithEnv(IDictionary<string, string?> env)
    {
        this.startInfo.Env = env;
        return this;
    }

    public Command SetEnv(string name, string value)
    {
        this.startInfo.SetEnv(name, value);
        return this;
    }

    public Command SetEnv(IEnumerable<KeyValuePair<string, string?>> values)
    {
        this.startInfo.SetEnv(values);
        return this;
    }

    public Command WithDisposable(IDisposable disposable)
    {
        this.startInfo.WithDisposable(disposable);
        return this;
    }

    public Command WithStdOut(Stdio stdio)
    {
        this.startInfo.Stdout = stdio;
        return this;
    }

    public Command WithStdErr(Stdio stdio)
    {
        this.startInfo.Stderr = stdio;
        return this;
    }

    public Command WithStdIn(Stdio stdio)
    {
        this.startInfo.Stdin = stdio;
        return this;
    }

    public Command WithStdio(Stdio stdio)
    {
        this.startInfo.Stdout = stdio;
        this.startInfo.Stderr = stdio;
        this.startInfo.Stdin = stdio;
        return this;
    }

    public Command WithVerb(string verb)
    {
        this.startInfo.Verb = verb;
        return this;
    }

    public Command AsWindowsAdmin()
    {
        this.startInfo.Verb = "runas";
        return this;
    }

    public Command AsSudo()
    {
        this.startInfo.Verb = "sudo";
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Command WithUser(string user)
    {
        this.startInfo.User = user;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Command WithPassword(string password)
    {
        this.startInfo.PasswordInClearText = password;
        return this;
    }

    [SupportedOSPlatform("windows")]
    public Command WithDomain(string domain)
    {
        this.startInfo.Domain = domain;
        return this;
    }

    public PsPipe Chain()
        => new(this.startInfo);

    public PsPipeAsync ChainAsync()
        => new(this.startInfo);

    public IEnumerable<string> Lines()
    {
        this.startInfo.WithStdOut(Stdio.Piped);
        return this.Output().Lines();
    }

    public async IAsyncEnumerable<string> LinesAsync(
        Encoding? encoding = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        this.startInfo.WithStdio(Stdio.Piped);
        var output = await this.OutputAsync(cancellationToken);
        await foreach (var line in output.LinesAsync(encoding, cancellationToken))
            yield return line;
    }

    public Result<IEnumerable<string>> LinesResult()
    {
        try
        {
            return Result.Ok(this.Lines());
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public ValueTask<Result<IAsyncEnumerable<string>>> LinesResultAsync(
        Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return Result.Ok(this.LinesAsync(encoding, cancellationToken));
        }
        catch (Exception ex)
        {
            return Result.Fail<IAsyncEnumerable<string>>(ex);
        }
    }

    public PsOutput Output()
    {
        this.startInfo.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        using var child = new PsChild(this.startInfo);
        var output = child.WaitForOutput();
        return output;
    }

    public async ValueTask<PsOutput> OutputAsync(CancellationToken cancellationToken = default)
    {
        this.startInfo.WithStdOut(Stdio.Piped)
            .WithStdErr(Stdio.Piped);

        using var child = new PsChild(this.startInfo);
        var r = await child.WaitForOutputAsync(cancellationToken);
        return r;
    }

    public Result<PsOutput> OutputResult()
    {
        try
        {
            return this.Output();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async ValueTask<Result<PsOutput>> OutputResultAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await this.OutputAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public PsPipe PipeTo(PsStartInfo startInfo)
        => this.Chain().PipeTo(startInfo);

    public PsPipe PipeTo(string fileName, PsArgs? args = null)
        => this.Chain().PipeTo(fileName, args);

    public PsPipe PipeTo(IPsStartInfoBuilder builder)
        => this.Chain().PipeTo(builder);

    public PsPipe PipeTo(PsChild next)
        => this.Chain().PipeTo(next);

    public PsPipeAsync PipeToAsync(PsStartInfo startInfo, CancellationToken cancellationToken = default)
        => this.ChainAsync().PipeTo(startInfo, cancellationToken);

    public PsPipeAsync PipeToAsync(string fileName, PsArgs? args = null, CancellationToken cancellationToken = default)
        => this.ChainAsync().PipeTo(fileName, args, cancellationToken);

    public PsPipeAsync PipeToAsync(IPsStartInfoBuilder builder, CancellationToken cancellationToken = default)
        => this.ChainAsync().PipeTo(builder, cancellationToken);

    public PsOutput Run()
    {
        using var child = new PsChild(this.startInfo);
        var output = child.WaitForOutput();
        return output;
    }

    public async ValueTask<PsOutput> RunAsync(CancellationToken cancellationToken = default)
    {
        using var child = new PsChild(this.startInfo);
        var output = await child.WaitForOutputAsync(cancellationToken).NoCap();
        return output;
    }

    public string Text(Encoding? encoding = null)
        => this.Output().Text(encoding);

    public async Task<string> TextAsync(Encoding? encoding = null, CancellationToken cancellationToken = default)
    {
        var output = await this.OutputAsync(cancellationToken);
        return output.Text(encoding);
    }

    public Result<string> TextResult(Encoding? encoding)
    {
        try
        {
            return this.OutputResult().Select(o => o.Text(encoding));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async ValueTask<Result<string>> TextResultAsync(Encoding? encoding, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await this.OutputResultAsync(cancellationToken);
            return result.Select(o => o.Text(encoding));
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public Result<PsOutput> RunResult()
    {
        try
        {
            return this.Run();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async ValueTask<Result<PsOutput>> RunResultAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await this.RunAsync(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public PsChild Spawn()
        => new(this.startInfo);

    PsStartInfo IPsStartInfoBuilder.Build()
        => this.startInfo;
}