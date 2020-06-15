using NStandard;
using System;
using System.Linq;
using System.Text;

namespace Chinese
{
    public static class ChineseNumber
    {
        public static readonly string[] UpperNumberValues = new[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
        public static readonly string[] LowerNumberValues = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
        public static readonly string[] LowerNumberPureValues = new[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        public static readonly string[] SuperiorLevels = new[] { "穰", "秭", "垓", "京", "兆", "亿", "万", "" };

        public static readonly string[] UpperLevels = new[] { "仟", "佰", "拾", "" };
        public static readonly string[] LowerLevels = new[] { "千", "百", "十", "" };
        public const int LEVEL_COUNT = 4;

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
        public static string GetString(decimal money, Action<ChineseNumberOptions> setOptions)
        {
            var options = new ChineseNumberOptions();
            setOptions(options);
            return GetString(money, options);
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
                        var singleNumberUnit = levels[LEVEL_COUNT - singles.Length + kv.Key];

                        if (zero)
                            sb.Append($"{numberValues[0]}{value}{singleNumberUnit}");
                        else sb.Append($"{value}{singleNumberUnit}");

                        zero = false;
                    }
                    else zero = true;
                }
                sb.Append(level);

                return sb.ToString();
            }

            //TODO: Use Linqsharp to calculate
            var levelParts = number.ToString()
                .For(parts => parts.AsKvPairs()
                    .GroupBy(x => (x.Key + (4 - parts.Length % 4)) / 4)
                    .Select(g => g.Select(x => x.Value).ToArray()))
                .ToArray();
            var ret = levelParts.Select((v, i) => GetPartString(v, SuperiorLevels[SuperiorLevels.Length - levelParts.Length + i])).Join("");

            if (!options.Verbose && (ret.StartsWith("一十") || ret.StartsWith("壹拾"))) ret = ret.Substring(1);

            return ret;
        }
    }
}
