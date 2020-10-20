using NStandard;
using System.Linq;

namespace Chinese
{
    public class ChineseLexicon : Scope<ChineseLexicon>
    {
        public static ChineseLexicon Default = new ChineseLexicon(BuiltinWords.Basic);

        public int WordMaxLength { get; }
        public ChineseWord[] Words { get; }

        public ChineseLexicon(ChineseWord[] words)
        {
            Words = words;
            WordMaxLength = Words.Max(x => x.Simplified.Length);
        }

        public ChineseLexicon(params ChineseWord[][] wordsSet)
        {
            Words = wordsSet.SelectMany(x => x).ToArray();
            WordMaxLength = Words.Max(x => x.Simplified.Length);
        }

    }
}
