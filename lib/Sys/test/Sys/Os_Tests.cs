using System.Runtime.InteropServices;

using static Gnome.Sys.Os;
using static Xunit.Assert;

namespace Tests;

public static class Os_Tests
{

    [UnitTest]
    public static void IsWindows_Test()
    {
        // Arrange
        var expected = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        // Act
        var result = IsWindows();

        // Assert
        Assert.Equal(expected, result);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Windows)]
    public static void IsWindowsVersionAtLeast_Test()
    {
        // Arrange
        var major = 10;
        var minor = 0;
        var build = 0;
        var revision = 0;

        // Act
        var result = IsWindowsVersionAtLeast(major, minor, build, revision);

        // Assert
        True(result);
    }

    [UnitTest]
    public static void IsLinux_Test()
    {
        // Arrange
        var expected = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        // Act
        var result = IsLinux();

        // Assert
        Assert.Equal(expected, result);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Linux)]
    public static void IsLinuxVersionAtLeast_Test()
    {
        // Arrange
        var major = 5;
        var minor = 4;
        var build = 0;
        var revision = 0;

        // Act
        var result = IsLinuxVersionAtLeast(major, minor, build, revision);

        // Assert
        True(result);
    }

    [UnitTest]
    public static void IsMacOS_Test()
    {
        // Arrange
        var expected = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        // Act
        var result = IsMacOS();

        // Assert
        Assert.Equal(expected, result);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.OSX)]
    public static void IsMacOSVersionAtLeast_Test()
    {
        // Arrange
        var major = 10;
        var minor = 0;
        var build = 0;

        // Act
        var result = IsMacOSVersionAtLeast(major, minor, build);

        // Assert
        True(result);
    }

    [UnitTest]
    [RequireOsPlatforms(TestOsPlatforms.Linux)]
    public static void IsVersionAtLeast_Test()
    {
        // Arrange
        var platform = "linux";
        var major = 5;
        var minor = 4;
        var build = 0;
        var revision = 0;

        // Act
        var result = IsVersionAtLeast(platform, major, minor, build, revision);

        // Assert
        True(result);
    }

    [UnitTest]
    public static void Is64Bit_Test()
    {
        // Arrange
        var expected = RuntimeInformation.OSArchitecture == Architecture.X64;

        // Act
        var result = Is64Bit;

        // Assert
        Assert.Equal(expected, result);
    }
}