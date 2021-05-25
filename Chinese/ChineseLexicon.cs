using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chinese
{
    public class ChineseLexicon
    {
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

        public LexiconScope BeginScope()
        {
            return new LexiconScope(this);
        }

    }
}
