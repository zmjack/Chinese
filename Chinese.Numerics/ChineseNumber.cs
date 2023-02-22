using Chinese.Core;
using NStandard;
using System;
using System.Linq;
using System.Text;

namespace Chinese.Numerics
{
    public static class ChineseNumber
    {
        private static Lexicon _lexicon = new(Builtin.NumericalWords);

        static ChineseNumber()
        {
            SuperiorLevels = new[] { "", "万", "亿", "兆", "京", "垓", "秭", "穰" };
        }

        public static readonly string[] UpperNumberValues = new[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        public static readonly string[] LowerNumberValues = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        public static readonly string[] LowerNumberCodeValues = new[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        private const int PART_COUNT = 4;
        private const int SUPERIOR_LEVELS_COUNT = 8;
        private static string[] superiorLevels;

        /// <summary>
        /// 自定义分级读法（简体中文，个位为空，从低到高设置八级），默认为 ["", "万", "亿", "兆", "京", "垓", "秭", "穰"]。
        /// </summary>
        public static string[] SuperiorLevels
        {
            get => superiorLevels;
            set
            {
                if (value.Length != SUPERIOR_LEVELS_COUNT) throw new ArgumentException("自定义分级读法必须设置八级。");

                superiorLevels = value;
                _lexicon = new Lexicon(Builtin.NumericalWords.Concat(value.Select(word =>
                {
                    var pinyin = string.Empty; // TODO: Pinyin.GetPinyin(word);
                    var chineseWord = new ChineseWord
                    {
                        Simplified = word,
                        Traditional = word,
                        SimplifiedPinyin = pinyin,
                        TraditionalPinyin = pinyin,
                    };
                    return chineseWord;
                })).ToArray());
            }
        }

        public static readonly string[] UpperLevels = new[] { "", "拾", "佰", "仟" };
        public static readonly string[] LowerLevels = new[] { "", "十", "百", "千" };

        /// <summary>
        /// 获取数字的编号读法。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static string GetCodeString(string number, bool upper = false)
        {
            if (!number.All(ch => '0' <= ch && ch <= '9')) throw new ArgumentException("不是合法的数字编号。");

            string[] numberValues;
            if (upper) numberValues = UpperNumberValues;
            else numberValues = LowerNumberCodeValues;

            var sb = new StringBuilder(number.Length + 1);
            foreach (var ch in number) sb.Append(numberValues[ch - '0']);

            return sb.ToString();
        }

        /// <summary>
        /// 获取数字的编号读法。
        /// </summary>
        /// <param name="chineseNumber"></param>
        /// <returns></returns>
        public static string GetCodeNumber(string chineseNumber)
        {
            var sb = new StringBuilder(chineseNumber.Length + 1);
            foreach (var ch in chineseNumber)
            {
                var value = LowerNumberCodeValues.IndexOf(ch.ToString());
                if (value == -1) value = UpperNumberValues.IndexOf(ch.ToString());

                if (value > -1) sb.Append(value);
                else throw new ArgumentException("不是合法的中文数字编号描述。", nameof(chineseNumber));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取数值的数值读法。
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GetString(decimal number) => GetString(number, ChineseNumberOptions.Default);

        /// <summary>
        /// 获取数值的数值读法。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="setOptions"></param>
        /// <returns></returns>
        /// 
        public static string GetString(decimal number, Action<ChineseNumberOptions> setOptions)
        {
            var options = new ChineseNumberOptions();
            setOptions(options);
            return GetString(number, options);
        }

        /// <summary>
        /// 获取数值的数值读法。
        /// </summary>
        /// <param name="number"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string GetString(decimal number, ChineseNumberOptions options)
        {
            number = decimal.Floor(number);

            string[] numberValues;
            string[] levels;
            if (options.Upper)
            {
                numberValues = UpperNumberValues;
                levels = UpperLevels;
            }
            else
            {
                numberValues = LowerNumberValues;
                levels = LowerLevels;
            }

            string GetPartString(char[] singles, string level, bool prevZero)
            {
                if (!singles.Any()) return string.Empty;

                var sb = new StringBuilder();
                var zero = prevZero;
                foreach (var (index, single) in singles.AsIndexValuePairs())
                {
                    if (single != '0')
                    {
                        var value = numberValues[single - '0'];
                        var singleNumberUnit = levels[singles.Length - 1 - index];

                        if (zero) sb.Append(numberValues[0]);
                        sb.Append($"{value}{singleNumberUnit}");

                        zero = false;
                    }
                    else zero = true;
                }

                if (sb.Length == 0) return null;
                else
                {
                    sb.Append(level);
                    return sb.ToString();
                }
            }

            var s_number = number.ToString();
            var enumerator_s_number = s_number.GetEnumerator();

            var levelParts = new char[(s_number.Length - 1) / 4 + 1][];
            var enumerator_levelParts = levelParts.GetEnumerator();
            if (enumerator_levelParts.MoveNext())
            {
                var mod = s_number.Length % PART_COUNT;
                levelParts[0] = new char[mod > 0 ? mod : 4];
                for (int j = 0; j < levelParts[0].Length; j++)
                {
                    enumerator_s_number.MoveNext();
                    levelParts[0][j] = enumerator_s_number.Current;
                }

                int i = 1;
                while (enumerator_levelParts.MoveNext())
                {
                    levelParts[i] = new char[4];
                    for (int j = 0; j < PART_COUNT; j++)
                    {
                        enumerator_s_number.MoveNext();
                        levelParts[i][j] = enumerator_s_number.Current;
                    }
                    i++;
                }
            }

            var sb = new StringBuilder();
            int part_i = 0;
            bool prevZero = false;
            foreach (var part in levelParts)
            {
                var partString = GetPartString(part, SuperiorLevels[levelParts.Length - 1 - part_i], prevZero);
                if (partString is not null)
                {
                    sb.Append(partString);
                    prevZero = false;
                }
                else prevZero = true;
                part_i++;
            }

            if (options.Simplified && sb.Length > 1)
            {
                if ((sb[0] == '一' && sb[1] == '十') || (sb[0] == '壹' && sb[1] == '拾'))
                {
                    sb.Remove(0, 1);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取数值读法的数值。
        /// </summary>
        /// <param name="chineseNumber"></param>
        /// <returns></returns>
        public static decimal GetNumber(string chineseNumber)
        {
            if (chineseNumber.Length == 0) return default;

            var last = chineseNumber.Last();
            if (last == '两') throw new ArgumentException($"不能以该字结尾：{last}", nameof(chineseNumber));

            var words = _lexicon.SplitWords(ChineseType.Simplified, chineseNumber);
            var total = 0m;
            var levelNumber = 0;
            foreach (var word in words)
            {
                if (word == "零") continue;
                else if (word == "十" || word == "拾") levelNumber += 10;
                else
                {
                    var numericalWord = _lexicon.GetWordFromCache(ChineseType.Simplified, word);
                    if (numericalWord != null) levelNumber += (int)numericalWord.Tag;
                    else
                    {
                        var level = superiorLevels.IndexOf(word);
                        if (level > -1)
                        {
                            total += levelNumber * (level switch
                            {
                                1 => 1_0000,
                                2 => 1_0000_0000,
                                3 => 1_0000_0000_0000,
                                4 => 1_0000_0000_0000_0000,
                                5 => 1_0000_0000_0000_0000_0000m,
                                6 => 1_0000_0000_0000_0000_0000_0000m,
                                7 => 1_0000_0000_0000_0000_0000_0000_0000m,
                                _ => throw new NotImplementedException(),
                            });
                            levelNumber = 0;
                        }
                        else throw new ArgumentException($"不能解析的词汇：{word}", nameof(chineseNumber));
                    }
                }
            }
            total += levelNumber;
            return total;
        }

    }
}
