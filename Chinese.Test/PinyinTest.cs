using Xunit;

namespace Chinese.Test
{
    public class PinyinTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("qu4 ba1£¬pi2 ka3 qiu1£¡", Pinyin.GetString("È¥°É£¬Æ¤¿¨Çð£¡", PinyinFormat.Default));
            Assert.Equal("qu ba£¬pi ka qiu£¡", Pinyin.GetString("È¥°É£¬Æ¤¿¨Çð£¡", PinyinFormat.WithoutTone));
            Assert.Equal("q¨´ b¨¡£¬p¨ª k¨£ qi¨±£¡", Pinyin.GetString("È¥°É£¬Æ¤¿¨Çð£¡", PinyinFormat.PhoneticSymbol));
        }
    }
}
