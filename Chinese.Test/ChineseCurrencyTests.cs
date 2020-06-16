using Xunit;

namespace Chinese.Test
{
    public class ChineseCurrencyTests
    {
        [Fact]
        public void LowerTest()
        {
            var options = new ChineseNumberOptions { Simplified = false, Upper = false };
            Assert.Equal("一十万零一元整", ChineseCurrency.GetString(10_0001, options));
            Assert.Equal("一十万零一百零一元整", ChineseCurrency.GetString(10_0101, options));
            Assert.Equal("一十万一千零一元整", ChineseCurrency.GetString(10_1001, options));
            Assert.Equal("一十万一千零一十元整", ChineseCurrency.GetString(10_1010, options));
            Assert.Equal("一十万零一元二角三分", ChineseCurrency.GetString(10_0001.23m, options));
            Assert.Equal("一十万零一元零三分", ChineseCurrency.GetString(10_0001.03m, options));
        }

        [Fact]
        public void SimplifiedLowerTest()
        {
            var options = new ChineseNumberOptions { Simplified = true, Upper = false };
            Assert.Equal("十万零一元整", ChineseCurrency.GetString(10_0001, options));
            Assert.Equal("十万零一百零一元整", ChineseCurrency.GetString(10_0101, options));
            Assert.Equal("十万一千零一元整", ChineseCurrency.GetString(10_1001, options));
            Assert.Equal("十万一千零一十元整", ChineseCurrency.GetString(10_1010, options));
            Assert.Equal("十万零一元二角整", ChineseCurrency.GetString(10_0001.2m, options));
            Assert.Equal("十万零一元二角三分", ChineseCurrency.GetString(10_0001.23m, options));
            Assert.Equal("十万零一元零三分", ChineseCurrency.GetString(10_0001.03m, options));
        }

        [Fact]
        public void UpperTest()
        {
            var options = new ChineseNumberOptions { Simplified = false, Upper = true };
            Assert.Equal("壹拾万零壹圆整", ChineseCurrency.GetString(10_0001, options));
            Assert.Equal("壹拾万零壹佰零壹圆整", ChineseCurrency.GetString(10_0101, options));
            Assert.Equal("壹拾万壹仟零壹圆整", ChineseCurrency.GetString(10_1001, options));
            Assert.Equal("壹拾万壹仟零壹拾圆整", ChineseCurrency.GetString(10_1010, options));
            Assert.Equal("壹拾万零壹圆贰角整", ChineseCurrency.GetString(10_0001.2m, options));
            Assert.Equal("壹拾万零壹圆贰角叁分", ChineseCurrency.GetString(10_0001.23m, options));
            Assert.Equal("壹拾万零壹圆零叁分", ChineseCurrency.GetString(10_0001.03m, options));
        }

        [Fact]
        public void SimplifiedUpperTest()
        {
            var options = new ChineseNumberOptions { Simplified = true, Upper = true };
            Assert.Equal("拾万零壹圆整", ChineseCurrency.GetString(10_0001, options));
            Assert.Equal("拾万零壹佰零壹圆整", ChineseCurrency.GetString(10_0101, options));
            Assert.Equal("拾万壹仟零壹圆整", ChineseCurrency.GetString(10_1001, options));
            Assert.Equal("拾万壹仟零壹拾圆整", ChineseCurrency.GetString(10_1010, options));
            Assert.Equal("拾万零壹圆贰角整", ChineseCurrency.GetString(10_0001.2m, options));
            Assert.Equal("拾万零壹圆贰角叁分", ChineseCurrency.GetString(10_0001.23m, options));
            Assert.Equal("拾万零壹圆零叁分", ChineseCurrency.GetString(10_0001.03m, options));
        }

    }
}
