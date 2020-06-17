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

        public static readonly string[] UpperNumberValues = new[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        public static readonly string[] LowerNumberValues = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        public static readonly string[] LowerNumberPureValues = new[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        private const int SUPERIOR_LEVELS_COUNT = 8;
        private static string[] _SuperiorLevels;

        /// <summary>
        /// 自定义分级读法（简体中文，个位为空，从低到高设置八级），默认为 ["", "万", "亿", "兆", "京", "垓", "秭", "穰"]。
        /// </summary>
        public static string[] SuperiorLevels
        {
            get => _SuperiorLevels;
            set
            {
                if (value.Length != SUPERIOR_LEVELS_COUNT) throw new ArgumentException("自定义分级读法必须设置八级。");

                _SuperiorLevels = value;
                NumericalWords = Words.NumericalWords.Concat(value.Select(word => new ChineseWord
                {
                    Pinyin = Pinyin.GetString(word),
                    Simplified = word,
                    Traditional = ChineseConverter.ToTraditional(word),
                })).ToArray();
            }
        }

        static ChineseNumber()
        {
            SuperiorLevels = new[] { "", "万", "亿", "兆", "京", "垓", "秭", "穰" };
        }

        public static readonly string[] UpperLevels = new[] { "", "拾", "佰", "仟" };
        public static readonly string[] LowerLevels = new[] { "", "十", "百", "千" };

        public static string GetPureString(decimal number, bool upper = false)
        {
            number = decimal.Floor(number);

            string[] numberValues;
            if (upper) numberValues = UpperNumberValues;
            else numberValues = LowerNumberPureValues;

            var str = number.ToString();
            var sb = new StringBuilder(str.Length + 1);
            foreach (var ch in str) sb.Append(numberValues[ch - '0']);

            return sb.ToString();
        }

        public static string GetString(decimal number) => GetString(number, ChineseNumberOptions.Default);
        public static string GetString(decimal number, Action<ChineseNumberOptions> setOptions)
        {
            var options = new ChineseNumberOptions();
            setOptions(options);
            return GetString(number, options);
        }
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

            //TODO: Use Linqsharp to calculate
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

        public static decimal GetNumber(string chineseNumber)
        {
            using (new ChineseLexicon(ChineseNumber.NumericalWords))
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
                        var numericalWord = Words.NumericalWords.FirstOrDefault(x => x.Simplified == word);
                        if (numericalWord != null) levelNumber += (int)numericalWord.Tag;
                        else
                        {
                            var level = _SuperiorLevels.IndexOf(word);
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
