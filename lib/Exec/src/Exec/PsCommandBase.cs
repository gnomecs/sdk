namespace Gnome.Exec;

public abstract class PsCommandBase : IPsStartInfoBuilder
{
    private PsStartInfo? args = null;

    PsStartInfo IPsStartInfoBuilder.Build()
        => this.BuildStartInfo();

    public PsCommandBase WithStartInfo(PsStartInfo startInfo)
    {
        this.args = startInfo;
        return this;
    }

    public PsStartInfo BuildStartInfo()
    {
        this.args ??= new PsStartInfo();
        this.args.FileName = this.GetExecutablePath();
        this.args.Args = this.BuildPsArgs();

        return this.args;
    }

    protected abstract string GetExecutablePath();

    protected virtual PsArgs BuildPsArgs()
        => new();
}