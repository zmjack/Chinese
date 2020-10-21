using Chinese;
using Microsoft.International.Converters.PinYinConverter;
using NStandard;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DictGenerator
{
    using Converter = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConverter;
    using Direction = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConversionDirection;

    class Program
    {
        public const string CJK_Radicals_Supplement = @"\u2E80-\u2EFF";
        public const string CJK_Unified_Ideographs_Extension_A = @"\u3400-\u4DBF";
        public const string CJK_Unified_Ideographs = @"\u4E00-\u9FFF";
        public const string CJK_Compatibility_Ideographs = @"\uF900-\uFAFF";

        static void Main(string[] args)
        {
            var chinese = EnumerableEx.Concat(
                RangeEx.Create(0x2E80, 0x2EFF - 0x2E80 + 1),
                RangeEx.Create(0x3400, 0x4DBF - 0x3400 + 1),
                RangeEx.Create(0x4E00, 0x9FFF - 0x4E00 + 1),
                RangeEx.Create(0xF900, 0xFAFF - 0xF900 + 1));
            var sb = new StringBuilder();

            var success = 0;
            var fail = 0;
            foreach (var c in chinese)
            {
                var ch = (char)c;
                var str = ch.ToString();

                try
                {
                    var pinyins = GetOriginPinyin(ch);
                    var simplified = Converter.Convert(str, Direction.TraditionalToSimplified);
                    var traditional = Converter.Convert(str, Direction.SimplifiedToTraditional);
                    var tag = "null";

                    sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyins = new[] {{ {pinyins.Select(pinyin => $@"""{pinyin}""").Join(", ")} }}, Simplified = ""{simplified}"", Traditional = ""{traditional}"", Tag = {tag} }},");
                    success++;
                }
                catch { fail++; }

                Console.WriteLine($"s:{success}\tf:{fail}\tt:{success + fail}");
            }

            var csFile = Build(sb.ToString());
            File.WriteAllText("../../../Basic.cs", csFile);

            Console.WriteLine("File saved: Basic.cs");
        }

        static string Build(string content)
        {
            return $@"
namespace Chinese
{{
    public static partial class BuiltinWords
    {{
        public static readonly ChineseWord[] Basic = new[]
        {{
{content}        }};

    }}
}}
";
        }

        public static string[] GetOriginPinyin(char ch)
        {
            var chineseChar = new ChineseChar(ch);
            var pinyins = chineseChar.Pinyins.OfType<string>().Select(pinyin => pinyin.ToLower()).ToArray();
            return pinyins;
        }

    }
}
