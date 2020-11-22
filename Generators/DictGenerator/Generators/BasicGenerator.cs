using Microsoft.International.Converters.PinYinConverter;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DictGenerator.Generators
{
    using Converter = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConverter;
    using Direction = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConversionDirection;

    public class BasicGenerator : IGenerator
    {
        //public const string CJK_Radicals_Supplement = @"\u2E80-\u2EFF";             // No use
        //public const string CJK_Unified_Ideographs_Extension_A = @"\u3400-\u4DBF";  // No use
        //public const string CJK_Unified_Ideographs = @"\u4E00-\u9FFF";
        //public const string CJK_Compatibility_Ideographs = @"\uF900-\uFAFF";        // No use

        public event Action<string> OnOutput;
        public void Output(string output) => OnOutput?.Invoke(output);

        public string Generate()
        {
            var outputs = new List<string>();
            var chinese = RangeEx.Create(0x4E00, 0x9FFF - 0x4E00 + 1);
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
                    var polyphonic = pinyins.Count(x => x.Last() != '5') > 1;

                    sb.AppendLine($@"{" ".Repeat(12)}new ChineseWord {{ Pinyins = new[] {{ {pinyins.Select(pinyin => $@"""{pinyin}""").Join(", ")} }}, Simplified = ""{simplified}"", Traditional = ""{traditional}"", Tag = {tag}, IsPolyphone = {(polyphonic ? "true" : "false")} }},");
                    success++;
                }
                catch
                {
                    fail++;
                }
                Output($"{str}\t{c,2:X2}\tSuccess: {success}\tFail   : {fail}\tTotal  : {success + fail}");
            }

            sb.AppendLine($@"{" ".Repeat(12)}// Total {success} words.");

            return Wrap(sb.ToString());
        }

        static string Wrap(string content)
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
