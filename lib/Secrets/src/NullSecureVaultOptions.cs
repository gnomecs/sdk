namespace Gnome.Secrets;

public class NullSecureVaultOptions : SecretVaultOptions
{
    public override Type SecretVaultType => typeof(NullSecretVault);
}