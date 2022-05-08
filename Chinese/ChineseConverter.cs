using System.Linq;
using System.Text;

namespace Chinese
{
    public static class ChineseConverter
    {
        /// <summary>
        /// 转换指定字符串到繁体中文。
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public static string ToTraditional(string chinese)
        {
            var scope = LexiconScope.Current ?? LexiconScope.Default;
            var lexicon = scope.Lexicon;
            var words = ChineseTokenizer.SplitWords(chinese);
            var sb = new StringBuilder(chinese.Length * 2);

            foreach (var word in words)
            {
                var result = lexicon.Find(ChineseTypes.Simplified, word)?.Traditional ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换指定字符串到简体中文。
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public static string ToSimplified(string chinese)
        {
            var scope = LexiconScope.Current ?? LexiconScope.Default;
            var lexicon = scope.Lexicon;
            var words = ChineseTokenizer.SplitWords(ChineseTypes.Simplified, chinese);
            var sb = new StringBuilder(chinese.Length * 2);

            foreach (var word in words)
            {
                var result = lexicon.Find(ChineseTypes.Traditional, word)?.Simplified ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

    }
}
