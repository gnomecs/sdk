using System.Collections;
using System.Globalization;

namespace Gnome.Sys;

public class EnvVariables : IEnvVariables
{
    public string this[string name]
    {
        get => this.Get(name);
        set => this.Set(name, value);
    }

    public string Expand(string value, EnvExpandOptions? options = null)
        => EnvVariablesExpander.Expand(value, options);

    public ReadOnlySpan<char> Expand(ReadOnlySpan<char> value, EnvExpandOptions? options = null)
        => EnvVariablesExpander.Expand(value, options);

    public Result<string> ExpandAsResult(string value, EnvExpandOptions? options = null)
        => EnvVariablesExpander.ExpandAsResult(value, options);

    public ValueResult<char[]> ExpandAsResult(ReadOnlySpan<char> value, EnvExpandOptions? options = null)
        => EnvVariablesExpander.ExpandAsResult(value, options);

    public string Get(string name)
        => System.Environment.GetEnvironmentVariable(name);

    public string Get(string name, EnvironmentVariableTarget windowsTarget)
        => System.Environment.GetEnvironmentVariable(name, windowsTarget);

    public bool TryGetValue(string name, out string value)
    {
        value = System.Environment.GetEnvironmentVariable(name);
        return value is not null && value.Length > 0;
    }

    public bool TryGetValue(string name, EnvironmentVariableTarget windowsTarget, out string value)
    {
        value = System.Environment.GetEnvironmentVariable(name, windowsTarget);
        return value is not null && value.Length > 0;
    }

    public bool Has(string name)
        => Environment.GetEnvironmentVariable(name) is not null;

    public bool Has(string name, EnvironmentVariableTarget windowsTarget)
        => Environment.GetEnvironmentVariable(name, windowsTarget) is not null;

    public void Set(string name, string value)
        => System.Environment.SetEnvironmentVariable(name, value);

    public void Set(string name, string value, EnvironmentVariableTarget windowsTarget)
        => System.Environment.SetEnvironmentVariable(name, value, windowsTarget);

    public void Remove(string name)
        => System.Environment.SetEnvironmentVariable(name, null);

    public void Remove(string name, EnvironmentVariableTarget windowsTarget)
        => System.Environment.SetEnvironmentVariable(name, null, windowsTarget);

    public IDictionary<string, string> ToDictionary()
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var dict = System.Environment.GetEnvironmentVariables();
        foreach (var key in dict.Keys)
        {
            if (key is not string k || string.IsNullOrWhiteSpace(k))
                continue;

            var rawValue = dict[key];
            if (rawValue is not string v || string.IsNullOrWhiteSpace(v))
                continue;

            map[k] = v;
        }

        return map;
    }
}