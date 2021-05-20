using Chinese.Options;
using NStandard;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Chinese
{
    public static class ChineseNumber
    {
        public static ChineseWord[] NumericalWords;

        static ChineseNumber()
        {
            SuperiorLevels = new[] { "", "万", "亿", "兆", "京", "垓", "秭", "穰" };
        }

        public static readonly string[] UpperNumberValues = new[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        public static readonly string[] LowerNumberValues = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        public static readonly string[] LowerNumberCodeValues = new[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

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
                NumericalWords = Builtin.NumericalWords.Concat(value.Select(word =>
                {
                    var pinyin = Pinyin.GetString(word);
                    var chineseWord = new ChineseWord
                    {
                        Simplified = word,
                        Traditional = ChineseConverter.ToTraditional(word),
                        SimplifiedPinyin = pinyin,
                        TraditionalPinyin = pinyin,
                    };
                    return chineseWord;
                })).ToArray();
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

            string GetPartString(char[] singles, string level)
            {
                if (!singles.Any()) return string.Empty;

                var sb = new StringBuilder();
                var zero = false;
                foreach (var kv in singles.AsKvPairs())
                {
                    if (kv.Value != '0')
                    {
                        var value = numberValues[kv.Value - '0'];
                        var singleNumberUnit = levels[singles.Length - 1 - kv.Key];

                        if (zero)
                            sb.Append($"{numberValues[0]}{value}{singleNumberUnit}");
                        else sb.Append($"{value}{singleNumberUnit}");

                        zero = false;
                    }
                    else zero = true;
                }

                if (sb.Length == 0) return numberValues[0];
                else
                {
                    sb.Append(level);
                    return sb.ToString();
                }
            }

            var sb = new StringBuilder();
            var levelParts = number.ToString()
                .For(parts => parts.AsKvPairs()
                    .GroupBy(x => (x.Key + (4 - parts.Length % 4)) / 4)
                    .Select(g => g.Select(x => x.Value).ToArray()))
                .ToArray();
            var chLevelParts = levelParts.Select((v, i) => GetPartString(v, SuperiorLevels[levelParts.Length - 1 - i]));
            var ret = chLevelParts.Join("").RegexReplace(new Regex($@"{numberValues[0]}{{2,}}"), numberValues[0]);

            if (options.Simplified && (ret.StartsWith("一十") || ret.StartsWith("壹拾"))) ret = ret.Substring(1);

            return ret;
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

            using (new ChineseLexicon(NumericalWords))
            {
                var words = ChineseTokenizer.SplitWords(chineseNumber);
                var total = 0m;
                var levelNumber = 0;
                foreach (var word in words)
                {
                    if (word == "零") continue;
                    else if (word == "十" || word == "拾") levelNumber += 10;
                    else
                    {
                        var numericalWord = Builtin.NumericalWords.FirstOrDefault(x => x.Simplified == word);
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
}
