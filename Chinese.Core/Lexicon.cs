using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chinese.Core
{
    public class Lexicon
    {
        public static Lexicon Default;

        private readonly HashSet<char> _charSet = new();
        private readonly HashSet<IWord> _wordSet = new();
        private int _maxWordLength = 0;

        private readonly List<IQueryable<IWord>> _sourceList = new();
        public int SourceCount => _sourceList.Count;

        public Lexicon(params IWord[][] wordsList)
        {
            foreach (var words in wordsList)
            {
                foreach (var word in words)
                {
                    CacheWord(word);
                    foreach (var ch in word.Simplified)
                    {
                        _charSet.Add(ch);
                    }
                }
            }
        }

        public Lexicon(params IQueryable<IWord>[] sourceList)
        {
            foreach (var source in sourceList)
            {
                _sourceList.Add(source);
            }
        }

        private void CacheWord(IWord word)
        {
            _wordSet.Add(word);
            var length = word.Simplified.Length;
            if (length > _maxWordLength) _maxWordLength = length;
        }

        public IWord GetWordFromCache(ChineseType type, string word)
        {
            if (type == ChineseType.Simplified)
                return _wordSet.FirstOrDefault(x => x.Simplified == word);
            else if (type == ChineseType.Traditional)
                return _wordSet.FirstOrDefault(x => x.Traditional == word);
            else throw new ArgumentException("The type must be specified.", nameof(type));
        }

        public IEnumerable<IWord> GetWords(string sentence)
        {
            IEnumerable<IWord> GetWordsForChar(char ch)
            {
                if (!_charSet.Contains(ch))
                {
                    var words = InnerGetWords(ch);
                    foreach (var word in words)
                    {
                        CacheWord(word);
                    }
                    _charSet.Add(ch);

                    return words;
                }
                else return _wordSet.Where(x => x.Simplified.Contains(ch));
            }

            foreach (var ch in sentence.Distinct())
            {
                foreach (var word in GetWordsForChar(ch))
                {
                    yield return word;
                }
            }
        }

        private IEnumerable<IWord> InnerGetWords(char ch)
        {
            var s_ch = ch.ToString();
            foreach (var source in _sourceList)
            {
                foreach (var word in source.Where(x => x.Simplified.Contains(s_ch)))
                {
                    yield return word;
                }
            }
        }

        /// <summary>
        /// 获取分词结果。
        /// </summary>
        /// <param name="chineseType"></param>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public string[] SplitWords(ChineseType chineseType, string chinese)
        {
            var words = GetWords(chinese).ToArray();

            var list = new LinkedList<string>();
            var length = chinese.Length;

            var maxOffset = Math.Min(chinese.Length, _maxWordLength);
            var ptext = length - maxOffset;
            var maxLengthPerTurn = maxOffset;
            int matchLength;
            for (; ptext + maxLengthPerTurn >= 0; ptext -= matchLength)
            {
                matchLength = 1;
                for (var i = ptext > 0 ? 0 : -ptext; i < maxLengthPerTurn; i++)
                {
                    var part = chinese.Substring(ptext + i, maxLengthPerTurn - i);
                    if (part.Length == 1)
                    {
                        list.AddFirst(part);
                        break;
                    }
                    else
                    {
                        bool isMatch = chineseType switch
                        {
                            ChineseType.Traditional => words.Any(x => x.Traditional == part),
                            _ => words.Any(x => x.Simplified == part),
                        };
                        if (isMatch)
                        {
                            list.AddFirst(part);
                            matchLength = part.Length;
                            break;
                        }
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 转换指定字符串到繁体中文。
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public string ToTraditional(string chinese)
        {
            var words = SplitWords(ChineseType.Simplified, chinese);
            var sb = new StringBuilder(chinese.Length * 2);

            foreach (var word in words)
            {
                var result = GetWordFromCache(ChineseType.Simplified, word)?.Traditional ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转换指定字符串到简体中文。
        /// </summary>
        /// <param name="chinese"></param>
        /// <returns></returns>
        public string ToSimplified(string chinese)
        {
            var words = SplitWords(ChineseType.Traditional, chinese);
            var sb = new StringBuilder(chinese.Length * 2);

            foreach (var word in words)
            {
                var result = GetWordFromCache(ChineseType.Traditional, word)?.Simplified ?? word;
                sb.Append(result);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取拼音（简体中文）
        /// </summary>
        /// <param name="chinese"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string GetPinyin(string chinese, PinyinFormat format = PinyinFormat.Default) => GetPinyin(ChineseType.Simplified, chinese, format);

        /// <summary>
        /// 获取指定类型字符串的拼音
        /// </summary>
        /// <param name="type"></param>
        /// <param name="chinese"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public string GetPinyin(ChineseType type, string chinese, PinyinFormat format = PinyinFormat.Default)
        {
            var steps = SplitWords(type, chinese).Select(x => x.Length);

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

                        if (pinyin is null && type.HasFlag(ChineseType.Simplified)) pinyin = GetWordFromCache(ChineseType.Simplified, word)?.SimplifiedPinyin;
                        if (pinyin is null && type.HasFlag(ChineseType.Traditional)) pinyin = GetWordFromCache(ChineseType.Traditional, word)?.TraditionalPinyin;

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
                            case PinyinFormat.InitialConsonant: sb.Append(GetPinyinInitialConsonant(pinyin)); break;
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

        private readonly static char[] one_to_five = new[] { '1', '2', '3', '4', '5' };

        internal static string GetPinyinInitialConsonant(string pinyin)
        {
            var parts = pinyin.Split(' ');
            var sb = new StringBuilder(parts.Length);
            foreach (var part in parts)
            {
                sb.Append(part[0]);
            }
            return sb.ToString();
        }
        internal static string GetPinyinWithoutTone(string pinyin)
        {
            var sb = new StringBuilder(pinyin.Length);
            foreach (var ch in pinyin)
            {
                if (!one_to_five.Contains(ch)) sb.Append(ch);
            }
            return sb.ToString();
        }
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
