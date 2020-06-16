using Xunit;

namespace Chinese.Test
{
    public class PinyinTest
    {
        [Fact]
        public void Test1()
        {
            var str = "免费，跨平台，开源！";
            Assert.Equal("mian3 fei4，kua4 ping2 tai1，kai1 yuan2！", Pinyin.GetString(str, PinyinFormat.Default));
            Assert.Equal("mian fei，kua ping tai，kai yuan！", Pinyin.GetString(str, PinyinFormat.WithoutTone));
            Assert.Equal("miǎn fèi，kuà píng tāi，kāi yuán！", Pinyin.GetString(str, PinyinFormat.PhoneticSymbol));
        }
    }
}
