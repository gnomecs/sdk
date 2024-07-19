using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Gnome.Sys.IO;

namespace Gnome.Exec;

public readonly struct PsOutput
{
    public PsOutput()
    {
        this.ExitCode = 0;
        this.FileName = string.Empty;
        this.Stdout = Array.Empty<byte>();
        this.Stderr = Array.Empty<byte>();
        this.StartTime = DateTime.MinValue;
        this.ExitTime = DateTime.MinValue;
    }

    public PsOutput(
        string fileName,
        int exitCode,
        byte[]? stdout = null,
        byte[]? stderr = null,
        DateTime? startTime = null,
        DateTime? exitTime = null)
    {
        this.FileName = fileName;
        this.ExitCode = exitCode;
        this.Stdout = stdout ?? Array.Empty<byte>();
        this.Stderr = stderr ?? Array.Empty<byte>();
        this.StartTime = startTime ?? DateTime.MinValue;
        this.ExitTime = exitTime ?? DateTime.MinValue;
    }

    public static PsOutput Empty { get; } = new();

    public int ExitCode { get; }

    public string FileName { get; }

    public byte[] Stdout { get; }

    public byte[] Stderr { get; }

    public DateTime StartTime { get; }

    public DateTime ExitTime { get; }

    public void Deconstruct(
        out int exitCode,
        out byte[] stdout,
        out byte[] stderr)
    {
        exitCode = this.ExitCode;
        stdout = this.Stdout;
        stderr = this.Stderr;
    }

    public void Deconstruct(
        out string fileName,
        out int exitCode,
        out byte[] stdout,
        out byte[] stderr,
        out DateTime startTime,
        out DateTime exitTime)
    {
        fileName = this.FileName;
        exitCode = this.ExitCode;
        stdout = this.Stdout;
        stderr = this.Stderr;
        startTime = this.StartTime;
        exitTime = this.ExitTime;
    }

    public string Text(Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(this.Stdout);
    }

    public string ErrorText(Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(this.Stderr);
    }

    public IEnumerable<string> Lines(Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        using var lines = new LinesEnumerator(this.Stdout, encoding);
        foreach (var line in lines)
            yield return line;
    }

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.")]
    public async IAsyncEnumerable<string> LinesAsync(
        Encoding? encoding = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        encoding ??= Encoding.UTF8;
        await using var lines = new LinesEnumerator(this.Stdout, encoding);
        await foreach (var line in lines)
            yield return line;
    }

    public IEnumerable<string> ErrorLines(Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        using var lines = new LinesEnumerator(this.Stderr, encoding);
        foreach (var line in lines)
            yield return line;
    }

    [SuppressMessage("Roslynator", "RCS1163:Unused parameter.")]
    public async IAsyncEnumerable<string> ErrorLinesAsync(
        Encoding? encoding = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        encoding ??= Encoding.UTF8;
        await using var lines = new LinesEnumerator(this.Stderr, encoding);
        await foreach (var line in lines)
            yield return line;
    }

    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public T? ErrorJson<T>(Encoding? encoding = null, JsonSerializerOptions? options = null)
    {
        if (this.Stderr.Length == 0)
            return default;

        options ??= new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

        return JsonSerializer.Deserialize<T>(this.Stderr, options);
    }

    public Dictionary<string, object?> ErrorJson(Encoding? encoding = null, JsonSerializerOptions? options = null)
    {
        if (this.Stderr.Length == 0)
            return new();

        options ??= new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

#pragma warning disable IL2026 // Dictionary<string, object>> should be part of the types.
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(this.Stderr, options) ?? new();
#pragma warning restore IL2026
    }

    [RequiresDynamicCode("JSON serialization and deserialization might require types that cannot be statically analyzed and might need runtime code generation. Use System.Text.Json source generation for native AOT applications.")]
    [RequiresUnreferencedCode("JSON serialization and deserialization might require types that cannot be statically analyzed. Use the overload that takes a JsonTypeInfo or JsonSerializerContext, or make sure all of the required types are preserved.")]
    public T? Json<T>(Encoding? encoding = null, JsonSerializerOptions? options = null)
    {
        if (this.Stdout.Length == 0)
            return default;

        options ??= new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

        return JsonSerializer.Deserialize<T>(this.Stdout, options);
    }

    public Dictionary<string, object?> Json(Encoding? encoding = null, JsonSerializerOptions? options = null)
    {
        if (this.Stdout.Length == 0)
            return new();

        options ??= new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };

#pragma warning disable IL2026 // Dictionary<string, object>> should be part of the types.
        return JsonSerializer.Deserialize<Dictionary<string, object?>>(this.Stdout, options) ?? new();
#pragma warning restore IL2026
    }

    public ValueResult ValidateAsResult(Func<int, bool>? validate = null)
    {
        if (validate is null)
        {
            if (this.ExitCode != 0)
                return ValueResult.Fail(new ProcessException(this.ExitCode, this.FileName));

            return ValueResult.Ok();
        }

        if (validate(this.ExitCode))
            return ValueResult.Ok();

        return ValueResult.Fail(new ProcessException(this.ExitCode, this.FileName));
    }

    public void Validate(Func<int, bool>? validate = null)
    {
        if (validate is null)
        {
            if (this.ExitCode != 0)
            {
                throw new ProcessException(this.ExitCode, this.FileName);
            }

            return;
        }

        if (!validate(this.ExitCode))
        {
            throw new ProcessException(this.ExitCode, this.FileName);
        }
    }

/*
    public Result<PsOutput, ProcessException> ToResult(Func<int, bool>? validate = null)
    {
        if (validate is null)
        {
            if (this.ExitCode != 0)
                return Result.Err<PsOutput, ProcessException>(new ProcessException(this.ExitCode, this.FileName));

            return Result.Ok<PsOutput, ProcessException>(this);
        }

        if (validate(this.ExitCode))
            return Result.Ok<PsOutput, ProcessException>(this);

        return Result.Err<PsOutput, ProcessException>(new ProcessException(this.ExitCode, this.FileName));
    }
    */
}