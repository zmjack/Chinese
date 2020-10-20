
namespace Chinese
{
    using System.Linq;
    using System.Text;

    public static class ChineseConverter
    {
        public static string ToTraditional(string chinese)
        {
            var lexicon = ChineseLexicon.Current ?? ChineseLexicon.Default;
            var words = ChineseTokenizer.SplitWords(chinese, ChineseType.Simplified);
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                var result = lexicon.Words.FirstOrDefault(x => x.Simplified == word)?.Traditional ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

        public static string ToSimplified(string chinese)
        {
            var lexicon = ChineseLexicon.Current ?? ChineseLexicon.Default;
            var words = ChineseTokenizer.SplitWords(chinese, ChineseType.Simplified);
            var sb = new StringBuilder();

            foreach (var word in words)
            {
                var result = lexicon.Words.FirstOrDefault(x => x.Traditional == word)?.Simplified ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

    }
}
