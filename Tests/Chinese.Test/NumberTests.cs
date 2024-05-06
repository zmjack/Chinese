using Chinese.Lexicons;
using System;
using Xunit;

namespace Chinese.Test;

public class NumberTests
{
    [Fact]
    public void Test()
    {
        var lexicon = Lexicon.Number;

        Assert.Equal("一", lexicon.GetString(1));
        Assert.Equal("一十万零一", lexicon.GetString(10_0001));
        Assert.Equal("一十万零一百零一", lexicon.GetString(10_0101));
        Assert.Equal("一十万一千零一", lexicon.GetString(10_1001));
        Assert.Equal("一十万一千零一十", lexicon.GetString(10_1010));

        Assert.Equal(1, lexicon.GetNumber("一"));
        Assert.Equal(10_0001, lexicon.GetNumber("一十万零一"));
        Assert.Equal(10_0101, lexicon.GetNumber("一十万零一百零一"));
        Assert.Equal(10_1001, lexicon.GetNumber("一十万一千零一"));
        Assert.Equal(10_1010, lexicon.GetNumber("一十万一千零一十"));
    }

    [Fact]
    public void UpperTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Upper);

        Assert.Equal("壹", lexicon.GetString(1));
        Assert.Equal("壹拾万零壹", lexicon.GetString(10_0001));
        Assert.Equal("壹拾万零壹佰零壹", lexicon.GetString(10_0101));
        Assert.Equal("壹拾万壹仟零壹", lexicon.GetString(10_1001));
        Assert.Equal("壹拾万壹仟零壹拾", lexicon.GetString(10_1010));

        Assert.Equal(1, lexicon.GetNumber("壹"));
        Assert.Equal(10_0001, lexicon.GetNumber("壹拾万零壹"));
        Assert.Equal(10_0101, lexicon.GetNumber("壹拾万零壹佰零壹"));
        Assert.Equal(10_1001, lexicon.GetNumber("壹拾万壹仟零壹"));
        Assert.Equal(10_1010, lexicon.GetNumber("壹拾万壹仟零壹拾"));
    }

    [Fact]
    public void ConciseTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Concise);

        Assert.Equal("一", lexicon.GetString(1));
        Assert.Equal("十万零一", lexicon.GetString(10_0001));
        Assert.Equal("十万零一百零一", lexicon.GetString(10_0101));
        Assert.Equal("十万一千零一", lexicon.GetString(10_1001));
        Assert.Equal("十万一千零一十", lexicon.GetString(10_1010));

        Assert.Equal(1, lexicon.GetNumber("一"));
        Assert.Equal(10_0001, lexicon.GetNumber("十万零一"));
        Assert.Equal(10_0101, lexicon.GetNumber("十万零一百零一"));
        Assert.Equal(10_1001, lexicon.GetNumber("十万一千零一"));
        Assert.Equal(10_1010, lexicon.GetNumber("十万一千零一十"));
    }

    [Fact]
    public void ConciseUpperTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Concise | NumberMode.Upper);

        Assert.Equal("壹", lexicon.GetString(1));
        Assert.Equal("拾万零壹", lexicon.GetString(10_0001));
        Assert.Equal("拾万零壹佰零壹", lexicon.GetString(10_0101));
        Assert.Equal("拾万壹仟零壹", lexicon.GetString(10_1001));
        Assert.Equal("拾万壹仟零壹拾", lexicon.GetString(10_1010));

        Assert.Equal(1, lexicon.GetNumber("壹"));
        Assert.Equal(10_0001, lexicon.GetNumber("拾万零壹"));
        Assert.Equal(10_0101, lexicon.GetNumber("拾万零壹佰零壹"));
        Assert.Equal(10_1001, lexicon.GetNumber("拾万壹仟零壹"));
        Assert.Equal(10_1010, lexicon.GetNumber("拾万壹仟零壹拾"));
    }

    [Fact]
    public void CodeTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Code);

        Assert.Equal("一〇〇〇〇一", lexicon.GetString(10_0001));
        Assert.Equal("一〇〇一〇一", lexicon.GetString(10_0101));
        Assert.Equal("一〇一〇〇一", lexicon.GetString(10_1001));
        Assert.Equal("一〇一〇一〇", lexicon.GetString(10_1010));

        Assert.Equal(10_0001, lexicon.GetNumber("一〇〇〇〇一"));
        Assert.Equal(10_0101, lexicon.GetNumber("一〇〇一〇一"));
        Assert.Equal(10_1001, lexicon.GetNumber("一〇一〇〇一"));
        Assert.Equal(10_1010, lexicon.GetNumber("一〇一〇一〇"));
    }

    [Fact]
    public void CodeUpperTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Code | NumberMode.Upper);

        Assert.Equal("壹零零零零壹", lexicon.GetString(10_0001));
        Assert.Equal("壹零零壹零壹", lexicon.GetString(10_0101));
        Assert.Equal("壹零壹零零壹", lexicon.GetString(10_1001));
        Assert.Equal("壹零壹零壹零", lexicon.GetString(10_1010));

        Assert.Equal(10_0001, lexicon.GetNumber("壹零零零零壹"));
        Assert.Equal(10_0101, lexicon.GetNumber("壹零零壹零壹"));
        Assert.Equal(10_1001, lexicon.GetNumber("壹零壹零零壹"));
        Assert.Equal(10_1010, lexicon.GetNumber("壹零壹零壹零"));
    }

    [Fact]
    public void BigIntegerTest()
    {
        var lexicon = Lexicon.Number;
        var expected = "一万亿亿亿二千三百四十五亿亿亿六千七百八十九万亿亿零一百二十三亿亿四千五百六十七万亿八千九百零一亿二千三百四十五万六千七百八十九";

        Assert.Equal(expected, lexicon.GetString(1_2345_6789_0123_4567_8901_2345_6789m));
        Assert.Equal(1_2345_6789_0123_4567_8901_2345_6789m, lexicon.GetNumber(expected));
    }

    [Fact]
    public void BigIntegerUpperTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Upper);
        var expected = "壹万亿亿亿贰仟叁佰肆拾伍亿亿亿陆仟柒佰捌拾玖万亿亿零壹佰贰拾叁亿亿肆仟伍佰陆拾柒万亿捌仟玖佰零壹亿贰仟叁佰肆拾伍万陆仟柒佰捌拾玖";

        Assert.Equal(expected, lexicon.GetString(1_2345_6789_0123_4567_8901_2345_6789m));
        Assert.Equal(1_2345_6789_0123_4567_8901_2345_6789m, lexicon.GetNumber(expected));
    }

    [Fact]
    public void BigIntegerTraditionalTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Classical);
        var expected = "一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九";

        Assert.Equal(expected, lexicon.GetString(1_2345_6789_0123_4567_8901_2345_6789m));
        Assert.Equal(1_2345_6789_0123_4567_8901_2345_6789m, lexicon.GetNumber(expected));
    }

    [Fact]
    public void BigIntegerTraditionalUpperTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Classical | NumberMode.Upper);
        var expected = "壹穰贰仟叁佰肆拾伍秭陆仟柒佰捌拾玖垓零壹佰贰拾叁京肆仟伍佰陆拾柒兆捌仟玖佰零壹亿贰仟叁佰肆拾伍万陆仟柒佰捌拾玖";

        Assert.Equal(expected, lexicon.GetString(1_2345_6789_0123_4567_8901_2345_6789m));
        Assert.Equal(1_2345_6789_0123_4567_8901_2345_6789m, lexicon.GetNumber(expected));
    }

    [Fact]
    public void MutilZeroTest()
    {
        var lexicon = Lexicon.Number;
        Assert.Equal("二十亿零一", lexicon.GetString(20_0000_0001));
        Assert.Equal("二十万亿零一", lexicon.GetString(20_0000_0000_0001));
        Assert.Equal(20_0000_0001, lexicon.GetNumber("二十亿零一"));
        Assert.Equal(20_0000_0000_0001, lexicon.GetNumber("二十万亿零一"));
    }

    [Fact]
    public void MutilZeroTraditionalTest()
    {
        var lexicon = Lexicon.NumberWith(NumberMode.Classical);
        Assert.Equal("二十亿零一", lexicon.GetString(20_0000_0001));
        Assert.Equal("二十兆零一", lexicon.GetString(20_0000_0000_0001));
        Assert.Equal(20_0000_0001, lexicon.GetNumber("二十亿零一"));
        Assert.Equal(20_0000_0000_0001, lexicon.GetNumber("二十兆零一"));
    }

    [Fact]
    public void AliasTest()
    {
        var lexicon = Lexicon.Number;
        Assert.Equal(22222, lexicon.GetNumber("两万二千两百二十二"));
        Assert.Throws<ArgumentException>(() => lexicon.GetNumber("两"));
        Assert.Throws<ArgumentException>(() => lexicon.GetNumber("两万二千两百二十两"));
    }

}
