using Chinese;
using NStandard;
using System;
using System.Text;

namespace BuiltinGenerator.Generators
{
    public class NumericalGenerator : IGenerator
    {
        public event Action<string> OnOutput;
        public void Output(string output) => OnOutput?.Invoke(output);

        public string Generate()
        {
            var sb = new StringBuilder();
            var levelGroups = new[] { ChineseNumber.UpperLevels, ChineseNumber.LowerLevels };
            var nums = new int[9].Let(i => i + 1);
            var total = 0;

            foreach (var levels in levelGroups.AsKvPairs())
            {
                foreach (var level in levels.Value.AsKvPairs())
                {
                    foreach (var num in nums)
                    {
                        var snumber = levels.Key == 0 ? ChineseNumber.GetString(num, x => x.Upper = true) : ChineseNumber.GetString(num);
                        var str = $"{snumber}{level.Value}";
                        var simplified = ChineseConverter.ToSimplified(str);
                        var traditional = ChineseConverter.ToTraditional(str);

                        var chineseWord = new ChineseWord
                        {
                            Simplified = simplified,
                            Traditional = traditional,
                            SimplifiedPinyin = Pinyin.GetString(ChineseType.Simplified, simplified),
                            TraditionalPinyin = Pinyin.GetString(ChineseType.Traditional, traditional),
                            Tag = num * Math.Pow(10, level.Key),
                        };

                        var line = $"{" ".Repeat(12)}new {nameof(ChineseWord)} " +
                            $"{{ " +
                            $"{nameof(ChineseWord.Simplified)} = \"{chineseWord.Simplified}\"" +
                            $", {nameof(ChineseWord.Traditional)} = \"{chineseWord.Traditional}\"" +
                            $", {nameof(ChineseWord.SimplifiedPinyin)} = \"{chineseWord.SimplifiedPinyin}\"" +
                            $", {nameof(ChineseWord.TraditionalPinyin)} = \"{chineseWord.TraditionalPinyin}\"" +
                            $", {nameof(ChineseWord.Tag)} = {chineseWord.Tag}" +
                            $" }},";
                        sb.AppendLine(line);
                        total += 1;
                    }
                }
            }

            sb.AppendLine($@"{" ".Repeat(12)}new {nameof(ChineseWord)} {{ Simplified = ""两"", Traditional = ""两"", SimplifiedPinyin = ""liang3"", TraditionalPinyin = ""liang3"", Tag = 2 }},");
            sb.AppendLine($@"{" ".Repeat(12)}new {nameof(ChineseWord)} {{ Simplified = ""两百"", Traditional = ""两百"", SimplifiedPinyin = ""liang3 bai3"", TraditionalPinyin = ""liang3 bai3"", Tag = 200 }},");
            sb.AppendLine($@"{" ".Repeat(12)}new {nameof(ChineseWord)} {{ Simplified = ""两千"", Traditional = ""两千"", SimplifiedPinyin = ""liang3 qian1"", TraditionalPinyin = ""liang3 qian1"", Tag = 2000 }},");
            total += 3;

            sb.AppendLine($@"{" ".Repeat(12)}// Total {total} words.");

            return Wrap(sb.ToString()).Replace("she4", "shi2");  // 拾 = shi2
        }

        static string Wrap(string content)
        {
            return $@"
namespace Chinese.Gen
{{
    public static partial class Builtin
    {{
        public static readonly ChineseWord[] NumericalWords = new[]
        {{
{content}        }};

    }}
}}
";
        }

    }
}
