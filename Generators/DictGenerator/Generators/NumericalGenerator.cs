using Chinese;
using NStandard;
using System;
using System.Text;

namespace DictGenerator.Generators
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
                        var pinyin = Pinyin.GetString(str);
                        var simplified = ChineseConverter.ToSimplified(str);
                        var traditional = ChineseConverter.ToTraditional(str);
                        var tag = num * Math.Pow(10, level.Key);
                        sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyins = new[] {{ ""{pinyin}"" }}, Simplified = ""{simplified}"", Traditional = ""{traditional}"", Tag = {tag} }},");
                        total += 1;
                    }
                }
            }

            sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyins = new[] {{ ""liang3"" }}, Simplified = ""两"", Traditional = ""两"", Tag = 2 }},");
            sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyins = new[] {{ ""liang3 bai3"" }}, Simplified = ""两百"", Traditional = ""两百"", Tag = 200 }},");
            sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyins = new[] {{ ""liang3 qian1"" }}, Simplified = ""两千"", Traditional = ""两千"", Tag = 2000 }},");
            total += 3;

            sb.AppendLine($@"{" ".Repeat(12)}// Total {total} words.");

            return Wrap(sb.ToString()).Replace("she4", "shi2");  // 拾 = shi2
        }

        static string Wrap(string content)
        {
            return $@"
namespace Chinese
{{
    public static partial class BuiltinWords
    {{
        public static readonly ChineseWord[] Numerical = new[]
        {{
{content}        }};

    }}
}}
";
        }

    }
}
