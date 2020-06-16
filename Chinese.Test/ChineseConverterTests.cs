using Xunit;

namespace Chinese.Test
{
    public class ChineseConverterTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("免費，跨平臺，開源！", ChineseConverter.ToTraditional("免费，跨平台，开源！"));
            Assert.Equal("免费，跨平台，开源！", ChineseConverter.ToSimplified("免費，跨平臺，開源！"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("皇后在國王后面。", ChineseConverter.ToTraditional("皇后在国王后面。"));

            var words = new[]
            {
                new ChineseWord { Pinyin = "huang2 hou4", Simplified = "皇后", Traditional = "皇后" },
                new ChineseWord { Pinyin = "hou4 mian4", Simplified = "后面", Traditional = "後面" },
            };

            using (new ChineseLexicon(words))
            {
                Assert.Equal("皇后在國王後面。", ChineseConverter.ToTraditional("皇后在国王后面。"));
            }
        }

    }
}
