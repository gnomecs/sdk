namespace Gnome.Sys;

/// <summary>
/// Represents a collection of environment variables and provides methods to interact with them.
/// </summary>
public interface IEnvVariables
{
    /// <summary>
    /// Gets or sets the value of the environment variable with the specified name.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    string this[string name] { get; set; }

    /// <summary>
    /// Expands environment variables in the specified string value.
    /// </summary>
    /// <param name="value">The string value containing environment variables to expand.</param>
    /// <param name="options">Optional options for expanding environment variables.</param>
    /// <returns>The expanded string value.</returns>
    string Expand(string value, EnvExpandOptions? options = null);

    /// <summary>
    /// Expands environment variables in the specified span of characters.
    /// </summary>
    /// <param name="value">The span of characters containing environment variables to expand.</param>
    /// <param name="options">Optional options for expanding environment variables.</param>
    /// <returns>The expanded span of characters.</returns>
    ReadOnlySpan<char> Expand(ReadOnlySpan<char> value, EnvExpandOptions? options = null);

    /// <summary>
    /// Expands environment variables in the specified string value and returns the result as a <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The string value containing environment variables to expand.</param>
    /// <param name="options">Optional options for expanding environment variables.</param>
    /// <returns>A <see cref="Result{T}"/> containing the expanded string value.</returns>
    Result<string> ExpandAsResult(string value, EnvExpandOptions? options = null);

    /// <summary>
    /// Expands environment variables in the specified span of characters and returns the result as a <see cref="ValueResult{T}"/>.
    /// </summary>
    /// <param name="value">The span of characters containing environment variables to expand.</param>
    /// <param name="options">Optional options for expanding environment variables.</param>
    /// <returns>A <see cref="ValueResult{T}"/> containing the expanded span of characters.</returns>
    ValueResult<char[]> ExpandAsResult(ReadOnlySpan<char> value, EnvExpandOptions? options = null);

    /// <summary>
    /// Gets the value of the environment variable with the specified name.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    string Get(string name);

    /// <summary>
    /// Gets the value of the environment variable with the specified name and target on Windows.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="windowsTarget">The target for the environment variable on Windows.</param>
    /// <returns>The value of the environment variable.</returns>
    string Get(string name, EnvironmentVariableTarget windowsTarget);

    /// <summary>
    /// Determines whether an environment variable with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <returns><c>true</c> if the environment variable exists; otherwise, <c>false</c>.</returns>
    bool Has(string name);

    /// <summary>
    /// Determines whether an environment variable with the specified name and target on Windows exists.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="windowsTarget">The target for the environment variable on Windows.</param>
    /// <returns><c>true</c> if the environment variable exists; otherwise, <c>false</c>.</returns>
    bool Has(string name, EnvironmentVariableTarget windowsTarget);

    /// <summary>
    /// Removes the environment variable with the specified name.
    /// </summary>
    /// <param name="name">The name of the environment variable to remove.</param>
    void Remove(string name);

    /// <summary>
    /// Removes the environment variable with the specified name and target on Windows.
    /// </summary>
    /// <param name="name">The name of the environment variable to remove.</param>
    /// <param name="windowsTarget">The target for the environment variable on Windows.</param>
    void Remove(string name, EnvironmentVariableTarget windowsTarget);

    /// <summary>
    /// Sets the value of the environment variable with the specified name.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="value">The value to set for the environment variable.</param>
    void Set(string name, string value);

    /// <summary>
    /// Sets the value of the environment variable with the specified name and target on Windows.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="value">The value to set for the environment variable.</param>
    /// <param name="windowsTarget">The target for the environment variable on Windows.</param>
    void Set(string name, string value, EnvironmentVariableTarget windowsTarget);

    /// <summary>
    /// Tries to get the value of the environment variable with the specified name.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="value">When this method returns, contains the value of the environment variable if it exists; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the environment variable exists; otherwise, <c>false</c>.</returns>
    bool TryGetValue(string name, out string value);

    /// <summary>
    /// Tries to get the value of the environment variable with the specified name and target on Windows.
    /// </summary>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="windowsTarget">The target for the environment variable on Windows.</param>
    /// <param name="value">When this method returns, contains the value of the environment variable if it exists; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the environment variable exists; otherwise, <c>false</c>.</returns>
    bool TryGetValue(string name, EnvironmentVariableTarget windowsTarget, out string value);

    /// <summary>
    /// Converts the collection of environment variables to a dictionary.
    /// </summary>
    /// <returns>A dictionary containing the environment variables.</returns>
    IDictionary<string, string> ToDictionary();
}