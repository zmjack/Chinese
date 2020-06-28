using Microsoft.International.Converters.PinYinConverter;
using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Chinese
{
    public static class Pinyin
    {
        public static string GetString(string chinese, PinyinFormat format = PinyinFormat.Default, ChineseType chineseType = ChineseType.Simplified)
        {
            var lexicon = ChineseLexicon.Current;
            IEnumerable<int> GetDefaultSteps() { foreach (var ch in chinese) yield return 1; }

            var steps = lexicon is null ? GetDefaultSteps() : ChineseTokenizer.SplitWords(chinese, chineseType).Select(x => x.Length);

            if (!chinese.IsNullOrWhiteSpace())
            {
                var sb = new StringBuilder();
                var insertSpace = false;
                var ptext = 0;
                foreach (var step in steps)
                {
                    var word = chinese.Substring(ptext, step);
                    try
                    {
                        string pinyin;
                        if (word.Length == 1)
                        {
                            var chineseChar = new ChineseChar(word[0]);
                            pinyin = chineseChar.Pinyins[0].ToString().ToLower();
                        }
                        else
                        {
                            var chineseWord = chineseType == ChineseType.Traditional
                                ? lexicon.Words.First(x => x.Traditional == word)
                                : lexicon.Words.First(x => x.Simplified == word);
                            pinyin = chineseWord.Pinyin;
                        }

                        if (format != PinyinFormat.Code)
                        {
                            if (insertSpace) sb.Append(" ");
                        }

                        switch (format)
                        {
                            case PinyinFormat.Default: sb.Append(pinyin); break;
                            case PinyinFormat.WithoutTone: sb.Append(GetPinyinWithoutTone(pinyin)); break;
                            case PinyinFormat.Phonetic: sb.Append(GetPhoneticSymbol(pinyin)); break;
                            case PinyinFormat.Code: sb.Append(pinyin.First()); break;
                        }
                        insertSpace = true;
                    }
                    catch
                    {
                        sb.Append(word);
                        insertSpace = false;
                    }

                    ptext += step;
                }

                return sb.ToString();
            }
            return chinese;
        }

        internal static string GetPinyinWithoutTone(string pinyin) => pinyin.Slice(0, -1);
        internal static string GetPhoneticSymbol(string pinyin)
        {
            var parts = pinyin.Split(' ').Select(part =>
            {
                var _pinyin = part.Slice(0, -1);
                var tone = int.Parse(part.Slice(-1));

                string yunmu;

                if (part.Contains("v")) yunmu = "v";
                else if (part.Contains("iu")) yunmu = "u";
                else if (part.Contains("ui")) yunmu = "i";
                else if (part.Contains("a")) yunmu = "a";
                else if (part.Contains("o")) yunmu = "o";
                else if (part.Contains("e")) yunmu = "e";
                else if (part.Contains("i")) yunmu = "i";
                else if (part.Contains("u")) yunmu = "u";
                else throw new InvalidCastException("Not a Pinyin.");

                var _yunmu = yunmu switch
                {
                    "a" => tone switch { 1 => "ā", 2 => "á", 3 => "ǎ", 4 => "à", _ => "a" },
                    "o" => tone switch { 1 => "ō", 2 => "ó", 3 => "ǒ", 4 => "ò", _ => "o" },
                    "e" => tone switch { 1 => "ē", 2 => "é", 3 => "ě", 4 => "è", _ => "e" },
                    "i" => tone switch { 1 => "ī", 2 => "í", 3 => "ǐ", 4 => "ì", _ => "i" },
                    "u" => tone switch { 1 => "ū", 2 => "ú", 3 => "ǔ", 4 => "ù", _ => "u" },
                    "v" => tone switch { 1 => "ǖ", 2 => "ǘ", 3 => "ǚ", 4 => "ǜ", _ => "ü" },
                };

                return _pinyin.Replace(yunmu, _yunmu);
            });

            return parts.Join(" ");
        }

    }
}
