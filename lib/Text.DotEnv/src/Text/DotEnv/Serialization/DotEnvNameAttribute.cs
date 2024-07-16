namespace Gnome.Text.DotEnv.Serialization;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class DotEnvNameAttribute : Attribute
{
    public DotEnvNameAttribute(string name)
    {
        this.Name = name;
    }

    public string Name { get; }

    public int Order { get; set; }
}