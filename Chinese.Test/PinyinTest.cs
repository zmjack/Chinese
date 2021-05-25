using Xunit;

namespace Chinese.Test
{
    public class PinyinTest
    {
        [Fact]
        public void TmpTest1()
        {
        }

        [Fact]
        public void Test1()
        {
            var str = "免费，跨平台，开源！";
            Assert.Equal("mian3 fei4，kua4 ping2 tai2，kai1 yuan2！", Pinyin.GetString(str, PinyinFormat.Default));
            Assert.Equal("mian fei，kua ping tai，kai yuan！", Pinyin.GetString(str, PinyinFormat.WithoutTone));
            Assert.Equal("miǎn fèi，kuà píng tái，kāi yuán！", Pinyin.GetString(str, PinyinFormat.Phonetic));
            Assert.Equal("mf，kpt，ky！", Pinyin.GetString(str, PinyinFormat.InitialConsonant));
        }

        [Fact]
        public void ChineseLexiconTest1()
        {
            var str = "他是来自重庆的重量级选手。";
            var pinyin = Pinyin.GetString(str, PinyinFormat.Default);
            Assert.Equal("ta1 shi4 lai2 zi4 zhong4 qing4 de5 zhong4 liang4 ji2 xuan3 shou3。", pinyin);

            var words = new ChineseWord[]
            {
                new ChineseWord { Simplified = "重庆", Traditional = "重慶", Pinyin = "chong2 qing4" },
                new ChineseWord { Simplified = "重量", Traditional = "重量", Pinyin = "zhong4 liang4" },
            };

            using (new ChineseLexicon(Builtin.ChineseChars, words).BeginScope())
            {
                Assert.Equal("ta1 shi4 lai2 zi4 chong2 qing4 de5 zhong4 liang4 ji2 xuan3 shou3。", Pinyin.GetString(str, PinyinFormat.Default));
            }
        }

        [Fact]
        public void ChineseLexiconTest2()
        {
            Assert.Equal("服務器中止了服務。", ChineseConverter.ToTraditional("服务器中止了服务。"));

            var words = new[]
            {
                new ChineseWord { Simplified = "服务器", Traditional = "伺服器", SimplifiedPinyin = "fu2 wu4 qi4", TraditionalPinyin = "si4 fu2 qi4" },
            };

            using (new ChineseLexicon(Builtin.ChineseChars, words).BeginScope())
            {
                Assert.Equal("伺服器中止了服務。", ChineseConverter.ToTraditional("服务器中止了服务。"));
                Assert.Equal("fu2 wu4 qi4 zhong1 zhi3 le5 fu2 wu4。", Pinyin.GetString(ChineseTypes.Simplified, "服务器中止了服务。"));
                Assert.Equal("si4 fu2 qi4 zhong1 zhi3 le5 fu2 wu4。", Pinyin.GetString(ChineseTypes.Traditional, "伺服器中止了服務。"));
            }
        }

    }
}
