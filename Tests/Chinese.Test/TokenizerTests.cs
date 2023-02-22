using Chinese.Core;
using Chinese.Test.Util;
using Xunit;

namespace Chinese.Test
{
    public class TokenizerTests
    {
        [Fact]
        public void Test1()
        {
            var lexicon = DefaultLexicon.Instance;
            var sentence = "中国北京是直辖市，重庆也是直辖市。";
            var actual = lexicon.SplitWords(ChineseType.Simplified, sentence);
            var excepted = new[] { "中国", "北京", "是", "直辖市", "，", "重庆", "也是", "直辖市", "。" };
            var pinyin = lexicon.GetPinyin(sentence, PinyinFormat.Phonetic);

            Assert.Equal(excepted, actual);
            Assert.Equal("zhōng guó běi jīng shì zhí xiá shì，chóng qìng yě shì zhí xiá shì。", pinyin);
        }

    }
}
