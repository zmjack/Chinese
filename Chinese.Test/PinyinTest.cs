using NStandard;
using System;
using Xunit;

namespace Chinese.Test
{
    public class PinyinTest
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("qu4 ba1£¬pi2 ka3 qiu1£¡", Pinyin.Get("È¥°É£¬Æ¤¿¨Çð£¡", PinyinFormat.Default));
            Assert.Equal("qu ba£¬pi ka qiu£¡", Pinyin.Get("È¥°É£¬Æ¤¿¨Çð£¡", PinyinFormat.WithoutTone));
            Assert.Equal("q¨´ b¨¡£¬p¨ª k¨£ qi¨±£¡", Pinyin.Get("È¥°É£¬Æ¤¿¨Çð£¡", PinyinFormat.PhoneticSymbol));
        }
    }
}
