using Gnome.Sys;

using static Gnome.Sys.Os;
using static Xunit.Assert;

namespace Tests;

// ReSharper disable once InconsistentNaming
public class Env_Tests
{
    [UnitTest]
    public void Get()
    {
        var result = Env.Get(IsWindows() ? "USERPROFILE" : "HOME");
        True(result.Length > 0);
    }
}