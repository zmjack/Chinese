using System;

namespace Chinese
{
    public static class ChineseCurrency
    {
        private static readonly string[] UpperLevels = new[] { "圆", "角", "分" };
        private static readonly string[] LowerLevels = new[] { "元", "角", "分" };

        public static string GetString(decimal money) => GetString(money, ChineseNumberOptions.Default);
        public static string GetString(decimal money, Action<ChineseNumberOptions> setOptions)
        {
            var options = new ChineseNumberOptions();
            setOptions(options);
            return GetString(money, options);
        }
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

    }
}
