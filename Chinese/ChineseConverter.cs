
namespace Chinese
{
    using System.Linq;
    using System.Text;
    using Converter = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConverter;
    using Direction = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConversionDirection;

    public static class ChineseConverter
    {
        public static string ToTraditional(string chinese)
        {
            var lexicon = ChineseLexicon.Current;
            if (lexicon is null) return Converter.Convert(chinese, Direction.SimplifiedToTraditional);

            var words = ChineseTokenizer.SplitWords(chinese, ChineseType.Simplified);
            var sb = new StringBuilder();
            foreach (var word in words)
            {
                if (word.Length == 1) sb.Append(Converter.Convert(word, Direction.SimplifiedToTraditional));
                else sb.Append(lexicon.Words.First(x => x.Simplified == word).Traditional);
            }
            return sb.ToString();
        }

        public static string ToSimplified(string chinese)
        {
            var lexicon = ChineseLexicon.Current;
            if (lexicon is null) return Converter.Convert(chinese, Direction.TraditionalToSimplified);

            var words = ChineseTokenizer.SplitWords(chinese, ChineseType.Simplified);
            var sb = new StringBuilder();
            foreach (var word in words)
            {
                if (word.Length == 1) sb.Append(Converter.Convert(word, Direction.TraditionalToSimplified));
                else sb.Append(lexicon.Words.First(x => x.Traditional == word).Simplified);
            }
            return sb.ToString();
        }

    }
}
