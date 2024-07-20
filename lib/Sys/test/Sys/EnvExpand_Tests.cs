using Gnome.Sys;

using static Gnome.Sys.Os;
using static Xunit.Assert;

namespace Tests;

// ReSharper disable once InconsistentNaming
public class EnvExpand_Tests
{
    [UnitTest]
    public void EvaluateNothing()
    {
        var result = Env.Expand("Hello World");
        Equal("Hello World", result);
    }

    [UnitTest]
    public void EvaluateEscapedName(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.Expand("Hello \\$WORD");
        Equal("Hello $WORD", result);

        result = Env.Expand("Hello $WORD\\_SUN");
        Equal("Hello World_SUN", result);
    }

    [UnitTest]
    public void EvaluateDoubleBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");
        Environment.SetEnvironmentVariable("HELLO", "Hello");

        var result = Env.Expand("$HELLO $WORD");
        Equal("Hello World", result);

        result = Env.Expand("$HELLO$WORD!");
        Equal("HelloWorld!", result);
    }

    [UnitTest]
    public void EvaluateSingleWindowsVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.Expand("Hello %WORD%");
        Equal("Hello World", result);

        result = Env.Expand("Hello test%WORD%:");
        Equal("Hello testWorld:", result);

        result = Env.Expand("%WORD%");
        Equal("World", result);

        result = Env.Expand("%WORD%  ");
        Equal("World  ", result);

        result = Env.Expand(" \n%WORD%  ");
        Equal(" \nWorld  ", result);
    }

    [UnitTest]
    public void EvaluateSingleBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.Expand("Hello $WORD");
        Equal("Hello World", result);

        result = Env.Expand("Hello test$WORD:");
        Equal("Hello testWorld:", result);

        result = Env.Expand("$WORD");
        Equal("World", result);

        result = Env.Expand("$WORD  ");
        Equal("World  ", result);

        result = Env.Expand(" \n$WORD  ");
        Equal(" \nWorld  ", result);
    }

    [UnitTest]
    public void EvaluateSingleInterpolatedBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var result = Env.Expand("Hello ${WORD}");
        Equal("Hello World", result);

        result = Env.Expand("Hello test${WORD}:");
        Equal("Hello testWorld:", result);

        result = Env.Expand("${WORD}");
        Equal("World", result);

        result = Env.Expand("${WORD}  ");
        Equal("World  ", result);

        result = Env.Expand(" \n$WORD  ");
        Equal(" \nWorld  ", result);
    }

    [UnitTest]
    public void UseDefaultValueForBashVar(IAssert assert)
    {
        // assert state
        False(Env.Has("WORD2"));

        var result = Env.Expand("${WORD2:-World}");
        Equal("World", result);
        False(Env.Has("WORD2"));
    }

    [UnitTest]
    public void SetEnvValueWithBashVarWhenNull(IAssert assert)
    {
        // assert state
        False(Env.Has("WORD3"));

        var result = Env.Expand("${WORD3:=World}");
        Equal("World", result);
        True(Env.Has("WORD3"));
        Equal("World", Env.Get("WORD3"));
    }

    [UnitTest]
    public void ThrowOnMissingBashVar(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        var ex = Throws<EnvExpandException>(() =>
        {
            Env.Expand("Hello ${WORLD:?WORLD must be set}");
        });

        Equal("WORLD must be set", ex.Message);

        ex = Throws<EnvExpandException>(() =>
        {
            Env.Expand("Hello ${WORLD}");
        });

        Equal("Bad substitution, variable WORLD is not set.", ex.Message);

        ex = Throws<EnvExpandException>(() =>
        {
            Env.Expand("Hello $WORLD");
        });

        Equal("Bad substitution, variable WORLD is not set.", ex.Message);
    }

    [UnitTest]
    public void UnclosedToken_Exception(IAssert assert)
    {
        Environment.SetEnvironmentVariable("WORD", "World");

        Throws<EnvExpandException>(() =>
        {
            Env.Expand("Hello ${WORD");
        });

        Throws<EnvExpandException>(() =>
        {
            Env.Expand("Hello %WORD");
        });
    }
}