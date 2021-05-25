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
            var words = ChineseTokenizer.SplitWords(chinese);
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                var result = scope.Lexicon.Words.FirstOrDefault(x => x.Simplified == word)?.Traditional ?? word;
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
            var words = ChineseTokenizer.SplitWords(ChineseTypes.Simplified, chinese);
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                var result = scope.Lexicon.Words.FirstOrDefault(x => x.Traditional == word)?.Simplified ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

    }
}
