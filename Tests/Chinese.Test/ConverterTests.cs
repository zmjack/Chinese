using Chinese.Test.Util;
using Xunit;

namespace Chinese.Test;

public class ConverterTests
{
    [Fact]
    public void Test1()
    {
        var lexicon = MySqlLexicon.UseDefault();
        Assert.Equal("免費，跨平臺，開源！", lexicon.ToTraditional("免费，跨平台，开源！"));
        Assert.Equal("免费，跨平台，开源！", lexicon.ToSimplified("免費，跨平臺，開源！"));
    }

    [Fact]
    public void Test2()
    {
        var lexicon = MySqlLexicon.UseDefault();
        Assert.Equal("皇后在國王後面。", lexicon.ToTraditional("皇后在国王后面。"));
    }

}
