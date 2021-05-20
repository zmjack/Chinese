using NStandard;
using System.Linq;

namespace Chinese
{
    public class ChineseLexicon : Scope<ChineseLexicon>
    {
        public static ChineseLexicon Default = new(Builtin.ChineseChars);

        public int WordMaxLength { get; }
        public ChineseWord[] Words { get; }

        public ChineseLexicon(ChineseWord[] words)
        {
            Words = words;
            WordMaxLength = Words.Any() ? Words.Max(x => x.Simplified?.Length ?? 0) : 0;
        }

        public ChineseLexicon(params ChineseWord[][] wordsSet)
        {
            Words = wordsSet.SelectMany(x => x).ToArray();
            WordMaxLength = Words.Any() ? Words.Max(x => x.Simplified?.Length ?? 0) : 0; ;
        }

    }
}
