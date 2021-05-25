using Chinese;
using NStandard;
using System;
using System.Collections.Generic;
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
                try
                {
                    var pinyins = GetOriginPinyins(ch);
                    if (pinyins.Skip(1).Any())
                    {
                        if (CharDefaultsModified.DefaultPinyins.ContainsKey(ch)) return CharDefaultsModified.DefaultPinyins[ch];
                        else if (CharDefaults.DefaultPinyins.ContainsKey(ch)) return CharDefaults.DefaultPinyins[ch];
                        else return pinyins.First();
                    }
                    else return pinyins.First();
                }
                catch
                {
                    return null;
                }
            }

            var charInts = RangeEx.Create(0x4E00, 0x9FFF - 0x4E00 + 1);
            var charList = new List<ChineseChar>();
            var success = 0;
            var fail = 0;
            foreach (var c in charInts)
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
                    var defaultPinyin = GetDefaultPinyin(ch) ?? GetDefaultPinyin(simplified[0]);

                    var chineseChar = new ChineseChar(ch, pinyins)
                    {
                        Simplified = simplified,
                        Traditional = traditional,
                        Pinyin = defaultPinyin,
                        Tag = null,
                    };
                    charList.Add(chineseChar);
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

            var sb = new StringBuilder();
            foreach (var item in charList)
            {
                var line = $"{" ".Repeat(12)}new {nameof(ChineseChar)}('{item.Char}', new[] {{{item.Pinyins.Select(x => $"\"{x}\"").Join(", ")}}}) " +
                    $"{{ " +
                    $"{nameof(ChineseChar.Simplified)} = \"{item.Simplified}\"" +
                    $", {nameof(ChineseChar.Traditional)} = \"{item.Traditional}\"" +
                    $", {nameof(ChineseChar.Pinyin)} = \"{item.SimplifiedPinyin}\"" +
                    $", {nameof(ChineseChar.Tag)} = null" +
                    $" }}";
                sb.AppendLine($@"{line}, // {(int)item.Char}");
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
