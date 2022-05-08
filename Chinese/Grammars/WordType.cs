#if NEW_FEATURE
using Chinese.Attributes;

namespace Chinese.Grammars
{
    public enum WordType
    {
        /// <summary>
        /// 未知词
        /// </summary>
        [Abbreviation("un")] Unknown,

        /// <summary>
        /// 形容词
        /// </summary>
        [Abbreviation("d")] Adjective,

        /// <summary>
        /// 连词
        /// </summary>
        [Abbreviation("c")] Conjunction,

        /// <summary>
        /// 叹词
        /// </summary>
        [Abbreviation("e")] Exclamation,

        /// <summary>
        /// 成语
        /// </summary>
        [Abbreviation("i")] Idiom,

        /// <summary>
        /// 数词
        /// </summary>
        [Abbreviation("e")] Numeral,

        /// <summary>
        /// 名词
        /// </summary>
        [Abbreviation("n")] Noun,

        /// <summary>
        /// 拟声词
        /// </summary>
        [Abbreviation("o")] Onomatopoeia,

        /// <summary>
        /// 介词
        /// </summary>
        [Abbreviation("p")] Prepositional,

        /// <summary>
        /// 量词
        /// </summary>
        [Abbreviation("q")] Quantity,

        /// <summary>
        /// 代词
        /// </summary>
        [Abbreviation("r")] Pronoun,

        /// <summary>
        /// 处所词
        /// </summary>
        [Abbreviation("s")] Space,

        /// <summary>
        /// 时间词
        /// </summary>
        [Abbreviation("t")] Time,

        /// <summary>
        /// 助词
        /// </summary>
        [Abbreviation("u")] Auxiliary,

        /// <summary>
        /// 动词
        /// </summary>
        [Abbreviation("v")] Verb,
    }

}
#endif
