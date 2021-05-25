using NStandard;
using System.Linq;

namespace Chinese
{
    public class LexiconScope : Scope<LexiconScope>
    {
        public ChineseLexicon Lexicon { get; private set; }

        public LexiconScope(ChineseLexicon lexicon)
        {
            Lexicon = lexicon;
        }

        public static LexiconScope Default = new(new ChineseLexicon(Builtin.ChineseChars));
    }
}
