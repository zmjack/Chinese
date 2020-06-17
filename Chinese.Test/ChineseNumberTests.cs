using Xunit;

namespace Chinese.Test
{
    public class ChineseNumberTests
    {
        [Fact]
        public void LowerTest()
        {
            var options = new ChineseNumberOptions { Simplified = false, Upper = false };
            Assert.Equal("一十万零一", ChineseNumber.GetString(10_0001, options));
            Assert.Equal("一十万零一百零一", ChineseNumber.GetString(10_0101, options));
            Assert.Equal("一十万一千零一", ChineseNumber.GetString(10_1001, options));
            Assert.Equal("一十万一千零一十", ChineseNumber.GetString(10_1010, options));
        }

        [Fact]
        public void SimplifiedLowerTest()
        {
            var options = new ChineseNumberOptions { Simplified = true, Upper = false };
            Assert.Equal("十万零一", ChineseNumber.GetString(10_0001, options));
            Assert.Equal("十万零一百零一", ChineseNumber.GetString(10_0101, options));
            Assert.Equal("十万一千零一", ChineseNumber.GetString(10_1001, options));
            Assert.Equal("十万一千零一十", ChineseNumber.GetString(10_1010, options));
        }

        [Fact]
        public void UpperTest()
        {
            var options = new ChineseNumberOptions { Simplified = false, Upper = true };
            Assert.Equal("壹拾万零壹", ChineseNumber.GetString(10_0001, options));
            Assert.Equal("壹拾万零壹佰零壹", ChineseNumber.GetString(10_0101, options));
            Assert.Equal("壹拾万壹仟零壹", ChineseNumber.GetString(10_1001, options));
            Assert.Equal("壹拾万壹仟零壹拾", ChineseNumber.GetString(10_1010, options));
        }

        [Fact]
        public void SimplifiedUpperTest()
        {
            var options = new ChineseNumberOptions { Simplified = true, Upper = true };
            Assert.Equal("拾万零壹", ChineseNumber.GetString(10_0001, options));
            Assert.Equal("拾万零壹佰零壹", ChineseNumber.GetString(10_0101, options));
            Assert.Equal("拾万壹仟零壹", ChineseNumber.GetString(10_1001, options));
            Assert.Equal("拾万壹仟零壹拾", ChineseNumber.GetString(10_1010, options));
        }

        [Fact]
        public void PureLowerTest()
        {
            Assert.Equal("一〇〇〇〇一", ChineseNumber.GetPureString(10_0001, upper: false));
            Assert.Equal("一〇〇一〇一", ChineseNumber.GetPureString(10_0101, upper: false));
            Assert.Equal("一〇一〇〇一", ChineseNumber.GetPureString(10_1001, upper: false));
            Assert.Equal("一〇一〇一〇", ChineseNumber.GetPureString(10_1010, upper: false));
        }

        [Fact]
        public void PureUpperTest()
        {
            Assert.Equal("壹零零零零壹", ChineseNumber.GetPureString(10_0001, upper: true));
            Assert.Equal("壹零零壹零壹", ChineseNumber.GetPureString(10_0101, upper: true));
            Assert.Equal("壹零壹零零壹", ChineseNumber.GetPureString(10_1001, upper: true));
            Assert.Equal("壹零壹零壹零", ChineseNumber.GetPureString(10_1010, upper: true));
        }

        [Fact]
        public void BigIntegerTest()
        {
            Assert.Equal("一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九", ChineseNumber.GetString(1_2345_6789_0123_4567_8901_2345_6789m));
            Assert.Equal("壹穰贰仟叁佰肆拾伍秭陆仟柒佰捌拾玖垓零壹佰贰拾叁京肆仟伍佰陆拾柒兆捌仟玖佰零壹亿贰仟叁佰肆拾伍万陆仟柒佰捌拾玖", ChineseNumber.GetString(1_2345_6789_0123_4567_8901_2345_6789m, x => x.Upper = true));
        }

        [Fact]
        public void MutilZeroTest()
        {
            Assert.Equal("二十亿零一", ChineseNumber.GetString(20_0000_0001));
            Assert.Equal("二十兆零一", ChineseNumber.GetString(20_0000_0000_0001));
        }

        [Fact]
        public void GetNumberTest()
        {
            Assert.Equal(10_0000_0001, ChineseNumber.GetNumber("十亿零一"));
            Assert.Equal(10_0000_0001, ChineseNumber.GetNumber("一十亿零一"));
            Assert.Equal(20_0000_0001, ChineseNumber.GetNumber("二十亿零一"));
            Assert.Equal(20_0000_0000_0001, ChineseNumber.GetNumber("二十兆零一"));
            Assert.Equal(1_2345_6789_0123_4567_8901_2345_6789m, ChineseNumber.GetNumber("一穰二千三百四十五秭六千七百八十九垓零一百二十三京四千五百六十七兆八千九百零一亿二千三百四十五万六千七百八十九"));
        }

        [Fact]
        public void TmpTest()
        {
            ChineseNumber.SuperiorLevels = new[] { "", "万", "亿", "万亿", "亿亿", "5", "6", "7" };
            var str = ChineseNumber.GetString(30_0000_0000_0001);
            var number = ChineseNumber.GetNumber("三十万亿零一");
        }

    }
}
