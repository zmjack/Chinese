using Xunit;

namespace Chinese.Test
{
    public class ChineseTokenizerTests
    {
        [Fact]
        public void Test1()
        {
            using var tokenizer = new ChineseLexicon(Builtin.ChineseChars, new[]
            {
                new ChineseWord { Simplified = "中国", Traditional = "中國", Pinyin = "zhong1 guo2" },
                new ChineseWord { Word = "北京", Pinyin = "bei3 jing1" },
                new ChineseWord { Simplified = "重庆", Traditional = "重慶", Pinyin = "chong2 qing4" },
                new ChineseWord { Simplified = "直辖市", Traditional = "直轄市", Pinyin = "zhi2 xia2 shi4" },
            }).BeginScope();

            var sentence = "中国北京是直辖市，重庆也是直辖市。";
            var actual = ChineseTokenizer.SplitWords(ChineseTypes.Simplified, sentence);
            var excepted = new[] { "中国", "北京", "是", "直辖市", "，", "重庆", "也", "是", "直辖市", "。" };
            var pinyin = Pinyin.GetString(sentence, PinyinFormat.Phonetic);

            Assert.Equal(excepted, actual);
            Assert.Equal("zhōng guó běi jīng shì zhí xiá shì，chóng qìng yě shì zhí xiá shì。", pinyin);
        }

    }
}
