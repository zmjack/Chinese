using Chinese.Lexicons;
using Xunit;

namespace Chinese.Test;

public class CurrencyTests
{
    [Fact]
    public void Test()
    {
        var lexicon = Lexicon.Currency;

        Assert.Equal("一元整", lexicon.GetString(1));
        Assert.Equal("一十万零一元整", lexicon.GetString(10_0001));
        Assert.Equal("一十万零一百零一元整", lexicon.GetString(10_0101));
        Assert.Equal("一十万一千零一元整", lexicon.GetString(10_1001));
        Assert.Equal("一十万一千零一十元整", lexicon.GetString(10_1010));
        Assert.Equal("一十万零一元二角整", lexicon.GetString(10_0001.2m));
        Assert.Equal("一十万零一元二角三分", lexicon.GetString(10_0001.23m));
        Assert.Equal("一十万零一元零三分", lexicon.GetString(10_0001.03m));

        Assert.Equal(1, lexicon.GetNumber("一元整"));
        Assert.Equal(10_0001, lexicon.GetNumber("一十万零一元整"));
        Assert.Equal(10_0101, lexicon.GetNumber("一十万零一百零一元整"));
        Assert.Equal(10_1001, lexicon.GetNumber("一十万一千零一元整"));
        Assert.Equal(10_1010, lexicon.GetNumber("一十万一千零一十元整"));
        Assert.Equal(10_0001.2m, lexicon.GetNumber("一十万零一元二角整"));
        Assert.Equal(10_0001.23m, lexicon.GetNumber("一十万零一元二角三分"));
        Assert.Equal(10_0001.03m, lexicon.GetNumber("一十万零一元零三分"));
    }

    [Fact]
    public void UpperTest()
    {
        var lexicon = Lexicon.CurrencyWith(NumberMode.Upper);

        Assert.Equal("壹圆整", lexicon.GetString(1));
        Assert.Equal("壹拾万零壹圆整", lexicon.GetString(10_0001));
        Assert.Equal("壹拾万零壹佰零壹圆整", lexicon.GetString(10_0101));
        Assert.Equal("壹拾万壹仟零壹圆整", lexicon.GetString(10_1001));
        Assert.Equal("壹拾万壹仟零壹拾圆整", lexicon.GetString(10_1010));
        Assert.Equal("壹拾万零壹圆贰角整", lexicon.GetString(10_0001.2m));
        Assert.Equal("壹拾万零壹圆贰角叁分", lexicon.GetString(10_0001.23m));
        Assert.Equal("壹拾万零壹圆零叁分", lexicon.GetString(10_0001.03m));

        Assert.Equal(1, lexicon.GetNumber("壹圆整"));
        Assert.Equal(10_0001, lexicon.GetNumber("壹拾万零壹圆整"));
        Assert.Equal(10_0101, lexicon.GetNumber("壹拾万零壹佰零壹圆整"));
        Assert.Equal(10_1001, lexicon.GetNumber("壹拾万壹仟零壹圆整"));
        Assert.Equal(10_1010, lexicon.GetNumber("壹拾万壹仟零壹拾圆整"));
        Assert.Equal(10_0001.2m, lexicon.GetNumber("壹拾万零壹圆贰角整"));
        Assert.Equal(10_0001.23m, lexicon.GetNumber("壹拾万零壹圆贰角叁分"));
        Assert.Equal(10_0001.03m, lexicon.GetNumber("壹拾万零壹圆零叁分"));
    }

    [Fact]
    public void LastZeroTest()
    {
        var lexicon = Lexicon.Currency;

        Assert.Equal("一十万元整", lexicon.GetString(10_0000m));
        Assert.Equal("一十亿元整", lexicon.GetString(10_0000_0000m));
        Assert.Equal("一十亿零一元整", lexicon.GetString(10_0000_0001m));
        Assert.Equal("一十亿零一千零一元整", lexicon.GetString(10_0000_1001m));
        Assert.Equal("一十亿零一百万一千零一元整", lexicon.GetString(10_0100_1001m));
        Assert.Equal("一千零二十万三千零四十元整", lexicon.GetString(1020_3040m));

        Assert.Equal(10_0000m, lexicon.GetNumber("一十万元整"));
        Assert.Equal(10_0000_0000m, lexicon.GetNumber("一十亿元整"));
        Assert.Equal(10_0000_0001m, lexicon.GetNumber("一十亿零一元整"));
        Assert.Equal(10_0000_1001m, lexicon.GetNumber("一十亿零一千零一元整"));
        Assert.Equal(10_0100_1001m, lexicon.GetNumber("一十亿零一百万一千零一元整"));
        Assert.Equal(1020_3040m, lexicon.GetNumber("一千零二十万三千零四十元整"));
    }

    [Fact]
    public void FirstZeroTest()
    {
        var lexicon = Lexicon.Currency;

        Assert.Equal("零元一角整", lexicon.GetString(0.1m));
        Assert.Equal("零元零一分", lexicon.GetString(0.01m));

        Assert.Equal(0.1m, lexicon.GetNumber("零元一角整"));
        Assert.Equal(0.01m, lexicon.GetNumber("零元零一分"));
    }

}
