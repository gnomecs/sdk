using System.Text.Json;
using System.Text.Json.Serialization;

using Gnome.Secrets;

namespace Gnome.Extensions.Secrets;

internal class JsonVault
{
    public JsonVaultSecretRecord[] Secrets { get; set; } = Array.Empty<JsonVaultSecretRecord>();
}

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(JsonVault))]
[JsonSerializable(typeof(JsonVaultSecretRecord))]
[JsonSerializable(typeof(JsonSecretRecord))]
[JsonSerializable(typeof(SecretRecord))]
[JsonSerializable(typeof(DateTime))]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(Dictionary<string, string?>))]
internal partial class JsonVaultSerializerContext : JsonSerializerContext
{
}

internal class JsonVaultSecretRecord : ISecretRecord
{
    public JsonVaultSecretRecord(JsonSecretRecord record)
    {
        this.Name = record.Name;
        this.Value = record.Value;
        this.ExpiresAt = record.ExpiresAt;
        this.CreatedAt = record.CreatedAt;
        this.UpdatedAt = record.UpdatedAt;
        this.Tags = record.Tags;
    }

    public string Name { get; set; }

    public string Value { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public IDictionary<string, string?> Tags { get; set; }
}

internal class JsonSecretRecord : SecretRecord
{
    public JsonSecretRecord(string name)
        : base(name)
    {
    }

    public JsonSecretRecord(ISecretRecord record)
        : base(record)
    {
    }

    internal JsonSecretRecord WithUpdatedAt(DateTime? updatedAt)
    {
        this.UpdatedAt = updatedAt;
        return this;
    }

    internal JsonSecretRecord WithCreatedAt(DateTime? createdAt)
    {
        this.CreatedAt = createdAt;
        return this;
    }

    internal void UpdateTags(IDictionary<string, string?> tags)
    {
        this.Tags = new Dictionary<string, string?>(tags, StringComparer.OrdinalIgnoreCase);
    }
}