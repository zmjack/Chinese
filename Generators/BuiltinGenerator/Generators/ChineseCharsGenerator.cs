using Chinese;
using NStandard;
using System;
using System.Linq;
using System.Text;

namespace BuiltinGenerator.Generators
{
    using Converter = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConverter;
    using Direction = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConversionDirection;
    using MicrosoftChineseChar = Microsoft.International.Converters.PinYinConverter.ChineseChar;

    public class ChineseCharsGenerator : IGenerator
    {
        //public const string CJK_Radicals_Supplement = @"\u2E80-\u2EFF";             // No use
        //public const string CJK_Unified_Ideographs_Extension_A = @"\u3400-\u4DBF";  // No use
        //public const string CJK_Unified_Ideographs = @"\u4E00-\u9FFF";
        //public const string CJK_Compatibility_Ideographs = @"\uF900-\uFAFF";        // No use

        public event Action<string> OnOutput;
        public void Output(string output) => OnOutput?.Invoke(output);

        public string Generate()
        {
            static string GetDefaultPinyin(char ch)
            {
                var pinyins = GetOriginPinyins(ch);
                if (pinyins.Skip(1).Any())
                {
                    if (MeasureWords.DefaultPinyins.ContainsKey(ch)) return MeasureWords.DefaultPinyins[ch];
                    else if (FrequencyWords.DefaultPinyins.ContainsKey(ch)) return FrequencyWords.DefaultPinyins[ch];
                    else return pinyins.First();
                }
                else return pinyins.First();
            }

            var chinese = RangeEx.Create(0x4E00, 0x9FFF - 0x4E00 + 1);
            var sb = new StringBuilder();

            var success = 0;
            var fail = 0;
            foreach (var c in chinese)
            {
                var ch = (char)c;
                var str = ch.ToString();
                var ignore = false;

            retry:
                try
                {
                    var pinyins = GetOriginPinyins(ch);
                    var simplified = Converter.Convert(str, Direction.TraditionalToSimplified);
                    var traditional = Converter.Convert(str, Direction.SimplifiedToTraditional);
                    var defaultPinyin = GetDefaultPinyin(simplified[0]);

                    var line = $"{" ".Repeat(12)}new {nameof(ChineseChar)}('{ch}', new[] {{{pinyins.Select(x => $"\"{x}\"").Join(", ")}}}) " +
                        $"{{ " +
                        $"{nameof(ChineseChar.Simplified)} = \"{simplified}\"" +
                        $", {nameof(ChineseChar.Traditional)} = \"{traditional}\"" +
                        $", {nameof(ChineseChar.Pinyin)} = \"{defaultPinyin}\"" +
                        $", {nameof(ChineseChar.Tag)} = null" +
                        $" }}";

                    sb.AppendLine($@"{line}, // {(int)ch}");
                    success++;
                }
                catch
                {
                    if (ignore == false)
                    {
                        ch = Converter.Convert(str, Direction.TraditionalToSimplified)[0];
                        ignore = true;
                        goto retry;
                    }
                    else fail++;
                }
                Output($"{str}\t{c,2:X2}\tSuccess: {success}\tFail   : {fail}\tTotal  : {success + fail}");
            }

            sb.AppendLine($@"{" ".Repeat(12)}// Total {success} words.");

            return Wrap(sb.ToString());
        }

        static string Wrap(string content)
        {
            return $@"
namespace Chinese.Gen
{{
    public static partial class Builtin
    {{
        public static readonly ChineseChar[] ChineseChars = new[]
        {{
{content}        }};

    }}
}}
";
        }

        public static string[] GetOriginPinyins(char ch)
        {
            var chineseChar = new MicrosoftChineseChar(ch);
            var pinyins = chineseChar.Pinyins.OfType<string>().Select(pinyin => pinyin.ToLower()).ToArray();
            return pinyins;
        }

    }
}
