using System.Globalization;

namespace Gnome.Sys;

public static class EnvVariableExtensions
{

    /// <summary>
    /// Retrieves the value of the specified environment variable as a boolean.
    /// </summary>
    /// <param name="env">The <see cref="IEnvVariables"/> instance.</param>
    /// <param name="name">The name of the environment variable.</param>
    /// <returns>The boolean value of the environment variable.</returns>
    /// <exception cref="System.ArgumentNullException">If the environment variable is not found.</exception>
    /// <exception cref="System.FormatException">If the environment variable is not a valid boolean.</exception>
    public static bool GetBool(this IEnvVariables env, string name)
        => bool.Parse(env.Get(name));

    public static bool GetBool(this IEnvVariables env, string name, bool defaultValue)
    {
        var value = env.Get(name);
        if (bool.TryParse(value, out var result))
            return result;

        return defaultValue;
    }

    public static char GetChar(this IEnvVariables env, string name, char defaultValue = '\0')
    {
        if (char.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static decimal GetDecimal(this IEnvVariables env, string name, decimal defaultValue = 0)
    {
        if (decimal.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static decimal GetDecimal(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, decimal defaultValue = 0)
    {
        if (decimal.TryParse(env.Get(name), styles, provider, out var result))
            return result;

        return defaultValue;
    }

    public static double GetDouble(this IEnvVariables env, string name, double defaultValue = 0)
    {
        if (double.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static double GetDouble(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, double defaultValue = 0)
    {
        if (double.TryParse(env.Get(name), styles, provider, out var result))
            return result;

        return defaultValue;
    }

    public static float GetFloat(this IEnvVariables env, string name, float defaultValue = 0)
    {
        if (float.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static float GetFloat(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, float defaultValue = 0)
    {
        if (float.TryParse(env.Get(name), styles, provider, out var result))
            return result;

        return defaultValue;
    }

    public static int GetInt16(this IEnvVariables env, string name, int defaultValue = 0)
    {
        if (short.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static int GetInt16(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, int defaultValue = 0)
    {
        if (short.TryParse(env.Get(name), styles, provider, out var result))
            return result;

        return defaultValue;
    }

    public static int GetInt32(this IEnvVariables env, string name, int defaultValue = 0)
    {
        if (int.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static int GetInt32(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, int defaultValue = 0)
    {
        if (int.TryParse(env.Get(name), styles, provider, out var result))
            return result;

        return defaultValue;
    }

    public static long GetInt64(this IEnvVariables env, string name, long defaultValue = 0)
    {
        if (long.TryParse(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static long GetInt64(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, long defaultValue = 0)
    {
        if (long.TryParse(env.Get(name), styles, provider, out var result))
            return result;

        return defaultValue;
    }

    public static DateTime GetDateTime(this IEnvVariables env, string name, DateTime defaultValue)
    {
#pragma warning disable S6580
        if (DateTime.TryParse(env.Get(name), out var result))
            return result;
#pragma warning restore S6580

        return defaultValue;
    }

    public static DateTime GetDateTime(this IEnvVariables env, string name, IFormatProvider provider, DateTimeStyles styles, DateTime defaultValue)
    {
        if (DateTime.TryParse(env.Get(name), provider, styles, out var result))
            return result;

        return defaultValue;
    }

    public static TimeSpan GetTimeSpan(this IEnvVariables env, string name, TimeSpan defaultValue)
    {
#pragma warning disable S6580
        if (TimeSpan.TryParse(env.Get(name), out var result))
            return result;
#pragma warning restore S6580

        return defaultValue;
    }

    public static TimeSpan GetTimeSpan(this IEnvVariables env, string name, IFormatProvider provider, TimeSpan defaultValue)
    {
        if (TimeSpan.TryParse(env.Get(name), provider, out var result))
            return result;

        return defaultValue;
    }

    public static TEnum GetEnum<TEnum>(this IEnvVariables env, string name)
        where TEnum : struct
        => (TEnum)Enum.Parse(typeof(TEnum), env.Get(name));

    public static TEnum GetEnum<TEnum>(this IEnvVariables env, string name, TEnum defaultValue)
        where TEnum : struct
    {
        if (Enum.TryParse<TEnum>(env.Get(name), out var result))
            return result;

        return defaultValue;
    }

    public static TEnum GetEnum<TEnum>(this IEnvVariables env, string name, bool ignoreCase, TEnum defaultValue)
        where TEnum : struct
    {
        if (Enum.TryParse<TEnum>(env.Get(name), ignoreCase, out var result))
            return result;

        return defaultValue;
    }

    public static bool TryParseBool(this IEnvVariables env, string name, out bool result)
        => bool.TryParse(env.Get(name), out result);

    public static bool TryParseChar(this IEnvVariables env, string name, out char result)
        => char.TryParse(env.Get(name), out result);

#pragma warning disable S6580
    public static bool TryParseDateTime(this IEnvVariables env, string name, out DateTime result)
        => DateTime.TryParse(env.Get(name), out result);
#pragma warning restore S6580

    public static bool TryParseDateTime(this IEnvVariables env, string name, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
        => DateTime.TryParse(env.Get(name), provider, styles, out result);

    public static bool TryParseDecimal(this IEnvVariables env, string name, out decimal result)
        => decimal.TryParse(env.Get(name), out result);

    public static bool TryParseDecimal(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, out decimal result)
        => decimal.TryParse(env.Get(name), styles, provider, out result);

    public static bool TryParseDouble(this IEnvVariables env, string name, out double result)
        => double.TryParse(env.Get(name), out result);

    public static bool TryParseDouble(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, out double result)
        => double.TryParse(env.Get(name), styles, provider, out result);

    public static bool TryParseEnum<TEnum>(this IEnvVariables env, string name, out TEnum result)
        where TEnum : struct
        => Enum.TryParse<TEnum>(env.Get(name), out result);

    public static bool TryParseFloat(this IEnvVariables env, string name, out float result)
        => float.TryParse(env.Get(name), out result);

    public static bool TryParseFloat(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, out float result)
        => float.TryParse(env.Get(name), styles, provider, out result);

    public static bool TryParseInt16(this IEnvVariables env, string name, out short result)
        => short.TryParse(env.Get(name), out result);

    public static bool TryParseInt16(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, out short result)
        => short.TryParse(env.Get(name), styles, provider, out result);

    public static bool TryParseInt32(this IEnvVariables env, string name, out int result)
        => int.TryParse(env.Get(name), out result);

    public static bool TryParseInt32(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, out int result)
        => int.TryParse(env.Get(name), styles, provider, out result);

    public static bool TryParseInt64(this IEnvVariables env, string name, out long result)
        => long.TryParse(env.Get(name), out result);

    public static bool TryParseInt64(this IEnvVariables env, string name, NumberStyles styles, IFormatProvider provider, out long result)
        => long.TryParse(env.Get(name), styles, provider, out result);

#pragma warning disable S6580
    public static bool TryParseTimeSpan(this IEnvVariables env, string name, out TimeSpan result)
        => TimeSpan.TryParse(env.Get(name), out result);
#pragma warning restore S6580

    public static bool TryParseTimeSpan(this IEnvVariables env, string name, IFormatProvider provider, out TimeSpan result)
        => TimeSpan.TryParse(env.Get(name), provider, out result);
}