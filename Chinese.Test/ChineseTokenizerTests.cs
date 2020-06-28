using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chinese.Test
{
    public class ChineseTokenizerTests
    {
        [Fact]
        public void Test1()
        {
            using var tokenizer = new ChineseLexicon(new[]
            {
                new ChineseWord { Pinyin = "zhong1 guo2", Simplified = "中国" },
                new ChineseWord { Pinyin = "bei3 jing1", Simplified = "北京" },
                new ChineseWord { Pinyin = "chong2 qing4", Simplified = "重庆" },
                new ChineseWord { Pinyin = "zhi2 xia2 shi4", Simplified = "直辖市" },
            });

            var sentence = "中国北京是直辖市，重庆也是直辖市。";
            var actual = ChineseTokenizer.SplitWords(sentence, ChineseType.Simplified);
            var excepted = new[] { "中国", "北京", "是", "直辖市", "，", "重庆", "也", "是", "直辖市", "。" };
            var pinyin = Pinyin.GetString(sentence, PinyinFormat.Phonetic);

            Assert.Equal(excepted, actual);
            Assert.Equal("zhōng guó běi jīng shì zhí xiá shì，chóng qìng yě shì zhí xiá shì。", pinyin);
        }

    }
}
