using System.Diagnostics;

namespace Gnome.Sys;

public static class CurrentProcess
{
    private static readonly Lazy<string[]> s_argv = new(() =>
    {
        var argv = new string[Environment.GetCommandLineArgs().Length - 1];
        var args = Environment.GetCommandLineArgs();
        Array.Copy(args, 1, argv, 0, args.Length - 1);
        return argv;
    });

    public static int ProcessorCount
        => Environment.ProcessorCount;

    public static int ExitCode
    {
        get => Environment.ExitCode;
        set => Environment.ExitCode = value;
    }

    public static string CommandLine
        => Environment.CommandLine;

    public static string[] Argv => s_argv.Value;

    public static string CurrentDirectory
    {
        get => Environment.CurrentDirectory;
        set => Environment.CurrentDirectory = value;
    }

#if !NETLEGACY
    public static bool IsPrivileged
        => Environment.IsPrivilegedProcess;
#endif

    public static int Id
    {
        get
        {
#if NETLEGACY
            return Process.GetCurrentProcess().Id;
#else
            return Environment.ProcessId;
#endif
        }
    }

    public static Process Get()
        => Process.GetCurrentProcess();

    public static void Exit(int exitCode)
        => Environment.Exit(exitCode);
}