using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chinese
{
    public static class Pinyin
    {
        /// <summary>
        /// 获取拼音（简体中文）
        /// </summary>
        /// <param name="chinese"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetString(string chinese, PinyinFormat format = PinyinFormat.Default) => GetString(ChineseTypes.Simplified, chinese, format);

        /// <summary>
        /// 获取指定类型字符串的拼音
        /// </summary>
        /// <param name="chineseTypes"></param>
        /// <param name="chinese"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetString(ChineseTypes chineseTypes, string chinese, PinyinFormat format = PinyinFormat.Default)
        {
            var scope = LexiconScope.Current ?? LexiconScope.Default;
            var lexicon = scope.Lexicon;
            IEnumerable<int> GetDefaultSteps() { foreach (var ch in chinese) yield return 1; }

            var steps = lexicon is null ? GetDefaultSteps() : ChineseTokenizer.SplitWords(chineseTypes, chinese).Select(x => x.Length);

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
                        string pinyin = null;

                        if (pinyin is null && chineseTypes.HasFlag(ChineseTypes.Simplified)) pinyin = lexicon.Words.FirstOrDefault(x => x.Simplified == word)?.SimplifiedPinyin;
                        if (pinyin is null && chineseTypes.HasFlag(ChineseTypes.Traditional)) pinyin = lexicon.Words.FirstOrDefault(x => x.Traditional == word)?.TraditionalPinyin;
                        if (pinyin is null && chineseTypes.HasFlag(ChineseTypes.Simplified)) pinyin = Builtin.ChineseChars.FirstOrDefault(x => x.Char == word[0])?.SimplifiedPinyin;
                        if (pinyin is null && chineseTypes.HasFlag(ChineseTypes.Traditional)) pinyin = Builtin.ChineseChars.FirstOrDefault(x => x.Char == word[0])?.TraditionalPinyin;

                        if (pinyin is null) throw new ArgumentException($"未能匹配文字（{word}）。");

                        if (format != PinyinFormat.InitialConsonant)
                        {
                            if (insertSpace) sb.Append(" ");
                        }

                        switch (format)
                        {
                            case PinyinFormat.Default: sb.Append(pinyin); break;
                            case PinyinFormat.WithoutTone: sb.Append(GetPinyinWithoutTone(pinyin)); break;
                            case PinyinFormat.Phonetic: sb.Append(GetPhoneticSymbol(pinyin)); break;
                            case PinyinFormat.InitialConsonant: sb.Append(pinyin.First()); break;
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
                    _ => throw new NotImplementedException(),
                };

                return _pinyin.Replace(yunmu, _yunmu);
            });

            return parts.Join(" ");
        }

    }
}
