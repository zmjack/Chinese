using Chinese.Test.Util;
using System.Linq;
using Xunit;

namespace Chinese.Test;

public class TokenizerTests
{
    [Fact]
    public void Test1()
    {
        var lexicon = MySqlLexicon.UseDefault();
        var sentence = "中国北京是直辖市，重庆也是直辖市。";

        var words = lexicon.SplitWords(sentence, ChineseType.Simplified);
        var exceptedWords = new[] { "中国", "北京", "是", "直辖市", "，", "重庆", "也是", "直辖市", "。" };
        Assert.Equal(exceptedWords, [.. words.Select(x => x.Simplified)]);

        var pinyin = lexicon.GetPinyin(sentence, PinyinFormat.Phonetic);
        Assert.Equal("zhōng guó běi jīng shì zhí xiá shì，chóng qìng yě shì zhí xiá shì。", pinyin);
    }

}
