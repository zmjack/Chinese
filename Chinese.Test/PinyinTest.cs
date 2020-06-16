using Xunit;

namespace Chinese.Test
{
    public class PinyinTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("mian3 fei4，kua4 ping2 tai1，kai1 yuan2！", Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.Default));
            Assert.Equal("mian fei，kua ping tai，kai yuan！", Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.WithoutTone));
            Assert.Equal("miǎn fèi，kuà píng tāi，kāi yuán！", Pinyin.GetString("免费，跨平台，开源！", PinyinFormat.PhoneticSymbol));
        }
    }
}
