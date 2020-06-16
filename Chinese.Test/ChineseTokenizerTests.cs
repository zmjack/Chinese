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
                new ChineseWord { Pinyin = "wo3 men1", Simplified = "我们" },
                new ChineseWord { Pinyin = "zhong1 guo2", Simplified = "中国" },
                new ChineseWord { Pinyin = "chong2 qing4", Simplified = "重庆" },
                new ChineseWord { Pinyin = "zhi2 xia2", Simplified = "直辖" },
            });

            var words = ChineseTokenizer.SplitWords("我们中国的北京是直辖市，我们中国的重庆也是直辖市", ChineseType.Simplified);
        }

    }
}
