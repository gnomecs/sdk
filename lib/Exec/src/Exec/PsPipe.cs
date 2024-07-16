namespace Gnome.Exec;

public class PsPipe
{
    private PsChild child;

    public PsPipe(PsCommandBase commandBase)
        : this(commandBase.BuildStartInfo())
    {
    }

    public PsPipe(PsStartInfo startInfo)
    {
        startInfo.WithStdio(Stdio.Piped);
        this.child = new PsChild(startInfo);
    }

    public PsPipe(PsChild child)
    {
        this.child = child;
    }

    public PsPipe PipeTo(string fileName, PsArgs? args = null)
        => this.PipeTo(new PsStartInfo(fileName, args ?? new PsArgs()));

    public PsPipe PipeTo(PsCommandBase commandBase)
        => this.PipeTo(commandBase.BuildStartInfo());

    public PsPipe PipeTo(IPsStartInfoBuilder startInfoBuilder)
        => this.PipeTo(startInfoBuilder.Build());

    public PsPipe PipeTo(PsStartInfo startInfo)
    {
        startInfo.WithStdio(Stdio.Piped);
        var next = new PsChild(startInfo);
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;

        return this;
    }

    public PsPipe PipeTo(PsChild next)
    {
        this.child.PipeTo(next);
        this.child.Wait();
        this.child.Dispose();
        this.child = next;

        return this;
    }

    public async Task<PsPipe> PipeAsync(PsChild next, CancellationToken cancellationToken = default)
    {
        await this.child.PipeToAsync(next, cancellationToken)
            .ConfigureAwait(false);
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public Task<PsPipe> PipeAsync(PsCommandBase commandBase, CancellationToken cancellationToken)
        => this.PipeAsync(commandBase.BuildStartInfo(), cancellationToken);

    public async Task<PsPipe> PipeAsync(PsStartInfo startInfo, CancellationToken cancellationToken = default)
    {
        startInfo.WithStdio(Stdio.Piped);
        var next = new PsChild(startInfo);
        await this.child.PipeToAsync(next, cancellationToken)
            .ConfigureAwait(false);
        this.child.Dispose();
        this.child = next;
        return this;
    }

    public Task<PsPipe> PipeAsync(string fileName, PsArgs? args = null, CancellationToken cancellationToken = default)
        => this.PipeAsync(new PsStartInfo(fileName, args ?? new PsArgs()), cancellationToken);

    public PsOutput Output()
    {
        return this.child.WaitForOutput();
    }

    public ValueTask<PsOutput> OutputAsync(CancellationToken cancellationToken = default)
    {
        return this.child.WaitForOutputAsync(cancellationToken);
    }
}