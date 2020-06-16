using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chinese
{
    public class ChineseLexicon : Scope<ChineseLexicon>
    {
        public int WordMaxLength { get; }
        public ChineseWord[] Words { get; }

        public ChineseLexicon(ChineseWord[] words)
        {
            Words = words;
            WordMaxLength = words.Max(x => x.Simplified.Length);
        }
    }
}
