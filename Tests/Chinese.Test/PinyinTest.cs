using Chinese.Core;
using Chinese.Test.Util;
using Xunit;

namespace Chinese.Test
{
    public class PinyinTest
    {
        [Fact]
        public void Test1()
        {
            var lexicon = DefaultLexicon.Instance;
            var str = "免费，跨平台，开源！";
            Assert.Equal("mian3 fei4，kua4 ping2 tai2，kai1 yuan2！", lexicon.GetPinyin(str, PinyinFormat.Default));
            Assert.Equal("mian fei，kua ping tai，kai yuan！", lexicon.GetPinyin(str, PinyinFormat.WithoutTone));
            Assert.Equal("miǎn fèi，kuà píng tái，kāi yuán！", lexicon.GetPinyin(str, PinyinFormat.Phonetic));
            Assert.Equal("mf，kpt，ky！", lexicon.GetPinyin(str, PinyinFormat.InitialConsonant));
        }

        [Fact]
        public void ChineseLexiconTest1()
        {
            var lexicon = DefaultLexicon.Instance;
            var str = "他是来自重庆的重量级选手。";
            Assert.Equal("ta1 shi4 lai2 zi4 chong2 qing4 de5 zhong4 liang4 ji2 xuan3 shou3。", lexicon.GetPinyin(str, PinyinFormat.Default));
        }

        [Fact]
        public void NotSameWordTest()
        {
            var lexicon = DefaultLexicon.Instance;
            Assert.Equal("伺服器中止了服務。", lexicon.ToTraditional("服务器中止了服务。"));
            Assert.Equal("fu2 wu4 qi4 zhong1 zhi3 le5 fu2 wu4。", lexicon.GetPinyin(ChineseType.Simplified, "服务器中止了服务。"));
            Assert.Equal("si4 fu2 qi4 zhong1 zhi3 le5 fu2 wu4。", lexicon.GetPinyin(ChineseType.Traditional, "伺服器中止了服務。"));
        }

    }
}
