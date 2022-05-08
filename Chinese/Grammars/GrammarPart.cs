#if NEW_FEATURE
namespace Chinese.Grammars
{
    public class GrammarPart
    {
        public WordType[][] WordClasses { get; private set; }
        private GrammarPart(WordType[][] wordClasses) { WordClasses = wordClasses; }

        public GrammarPart Subject = new GrammarPart(new[]
        {
            new[] { WordType.Noun },
            new[] { WordType.Pronoun },
            new[] { WordType.Time },
            new[] { WordType.Adjective },
            new[] { WordType.Verb },
            new[] { WordType.Numeral },
            new[] { WordType.Numeral, WordType.Quantity },
        });
        public GrammarPart Predicate = new GrammarPart(new[]
        {
            new[] { WordType.Verb },
        });
    }
}
#endif
