using Chinese;
using NStandard;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace NumericalWordsGenerator
{
    class Program
    {
        static string Build(string content)
        {
            return $@"
namespace Chinese
{{
    public static partial class Words
    {{
        public static readonly ChineseWord[] NumericalWords = new[]
        {{
{content}        }};

    }}
}}
";
        }

        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            var levelGroups = new[] { ChineseNumber.UpperLevels, ChineseNumber.LowerLevels };
            var nums = new int[9].Let(i => i + 1);

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
                        sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyin = ""{pinyin}"", Simplified = ""{simplified}"", Traditional = ""{traditional}"", Tag = {tag} }},");
                    }
                }
            }

            var csFile = Build(sb.ToString()).Replace("she4", "shi2");  // 拾 = shi2
            File.WriteAllText("../../../NumberWords.cs", csFile);

            Console.WriteLine("File saved: NumberWords.cs");
        }
    }
}
