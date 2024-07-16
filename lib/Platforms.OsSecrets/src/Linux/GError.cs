using System.Runtime.InteropServices;

namespace Gnome.Platforms.Linux;

[StructLayout(LayoutKind.Sequential)]
internal struct GError
{
    public uint Domain;

    public int Code;

    public IntPtr Message;
}