using NStandard;
using System;
using System.Text.RegularExpressions;

namespace Chinese
{
    public static class ChineseCurrency
    {
        private static readonly string[] UpperLevels = new[] { "圆", "角", "分" };
        private static readonly string[] LowerLevels = new[] { "元", "角", "分" };

        /// <summary>
        /// 获取数值的货币读法。
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public static string GetString(decimal money) => GetString(money, ChineseNumberOptions.Default);
        /// <summary>
        /// 获取数值的货币读法。
        /// </summary>
        /// <param name="money"></param>
        /// <param name="setOptions"></param>
        /// <returns></returns>
        public static string GetString(decimal money, Action<ChineseNumberOptions> setOptions)
        {
            var options = new ChineseNumberOptions();
            setOptions(options);
            return GetString(money, options);
        }
        /// <summary>
        /// 获取数值的货币读法。
        /// </summary>
        /// <param name="money"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string GetString(decimal money, ChineseNumberOptions options)
        {
            var fractional100 = (int)(money % 1 * 100 % 100);

            string[] numberValues;
            string[] levels;
            if (options.Upper)
            {
                numberValues = ChineseNumber.UpperNumberValues;
                levels = UpperLevels;
            }
            else
            {
                numberValues = ChineseNumber.LowerNumberValues;
                levels = LowerLevels;
            }

            var yuan = ChineseNumber.GetString(money, options);
            string ret;

            if (fractional100 == 0) ret = $"{yuan}{levels[0]}整";
            else if (fractional100 % 10 == 0) ret = $"{yuan}{levels[0]}{numberValues[fractional100 / 10]}{levels[1]}整";
            else
            {
                var jiao = fractional100 / 10;
                ret = $"{yuan}{levels[0]}{(jiao > 0 ? $"{numberValues[jiao]}{levels[1]}" : numberValues[0])}{numberValues[fractional100 % 10]}{levels[2]}";
            }

            return ret;
        }

        /// <summary>
        /// 获取货币读法的数值。
        /// </summary>
        /// <param name="chineseCurrency"></param>
        /// <returns></returns>
        public static decimal GetNumber(string chineseCurrency)
        {
            var regex = new Regex(@"(.+)(?:圆|元)(?:整|(.)角整|(.)角(.)分|零(.)分)");
            var match = regex.Match(chineseCurrency);

            if (!match.Success) throw new ArgumentException("不是合法的中文货币描述。", nameof(chineseCurrency));

            var yuan = ChineseNumber.GetNumber(match.Groups[1].Value);
            var jiao = (match.Groups[2].Value.For(x => x.IsWhiteSpace() ? null : x)
                     ?? match.Groups[3].Value.For(x => x.IsWhiteSpace() ? null : x))
                     ?.For(x => ChineseNumber.GetNumber(x)) ?? 0m;
            var fen = (match.Groups[4].Value.For(x => x.IsWhiteSpace() ? null : x)
                     ?? match.Groups[5].Value.For(x => x.IsWhiteSpace() ? null : x))
                     ?.For(x => ChineseNumber.GetNumber(x)) ?? 0m;

            return yuan + (jiao * 0.1m) + (fen * 0.01m);
        }

    }
}
