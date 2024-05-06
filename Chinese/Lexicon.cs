using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqSharp;
using Chinese.Lexicons;
using Microsoft.Extensions.Caching.Memory;

namespace Chinese;

public class Lexicon
{
    private static ArgumentException NotSupportedChineseType(ChineseType chineseType, string argument) => new($"不支持的简繁类型（{chineseType}）。", argument);

    private readonly HashSet<IWord> _wordSet = [];
    private int _maxWordLength = 0;
    private const int COMBINE_QUERY_COUNT = 100;
    private readonly List<IQueryable<IWord>> _sourceList = [];

    private static readonly Lazy<NumberLexicon> _number = new(() => new(new()));
    public static NumberLexicon Number => _number.Value;

    private static readonly MemoryCache _numberCache = new(new MemoryCacheOptions());
    public static NumberLexicon NumberWith(NumberMode mode)
    {
        return _numberCache.GetOrCreate(mode, entry =>
        {
            return new NumberLexicon(mode);
        });
    }

    private static readonly Lazy<CurrencyLexicon> _currency = new(() => new(new()));
    public static CurrencyLexicon Currency => _currency.Value;

    private static readonly MemoryCache _currencyCache = new(new MemoryCacheOptions());
    public static CurrencyLexicon CurrencyWith(NumberMode mode)
    {
        return _currencyCache.GetOrCreate(mode, entry =>
        {
            return new CurrencyLexicon(mode);
        });
    }

    private IEnumerable<IEnumerable<IWord>> GetSources()
    {
        yield return _wordSet;

        foreach (var source in _sourceList)
        {
            yield return source;
        }
    }

    public Lexicon()
    {
    }

    public void Add(IWord[] words)
    {
        foreach (var word in words)
        {
            _wordSet.Add(word);
            var length = Math.Max(word.Simplified.Length, word.Traditional.Length);
            if (length > _maxWordLength)
            {
                _maxWordLength = length;
            }
        }
    }

    public void Add(IQueryable<IWord> source)
    {
        _sourceList.Add(source);

        var length = source.Max(x => Math.Max(x.Simplified.Length, x.Traditional.Length));
        if (length > _maxWordLength)
        {
            _maxWordLength = length;
        }
    }

    public IEnumerable<IWord> GetWords(ChineseType chineseType, string sentence)
    {
        if (sentence.Any())
        {
            var length = sentence.Length;
            for (int i = 0; i < length; i += COMBINE_QUERY_COUNT)
            {
                var prev = i - 1;
                var start = prev < 0 ? 0 : prev;
                var end = i + COMBINE_QUERY_COUNT;
                end = end <= length ? end : length;

                var chunk = sentence[start..end];
                foreach (var word in GetSourceWords(chineseType, chunk))
                {
                    yield return word;
                }
            }

            var sources = GetSources();
            var last = sentence[^1].ToString();
            foreach (var source in sources)
            {
                if (chineseType == ChineseType.Simplified)
                {
                    var find = source.FirstOrDefault(x => x.Simplified == last);
                    if (find is not null) yield return find;
                }
                else if (chineseType == ChineseType.Traditional)
                {
                    var find = source.FirstOrDefault(x => x.Traditional == last);
                    if (find is not null) yield return find;
                }
                else throw NotSupportedChineseType(chineseType, nameof(chineseType));
            }
        }
    }

    private IEnumerable<IWord> GetSourceWords(ChineseType chineseType, string chunk)
    {
        var sources = GetSources();
        foreach (IEnumerable<IWord> source in sources)
        {
            IWord[] queryResult;

            if (chineseType == ChineseType.Simplified)
            {
                queryResult =
                [
                    ..
                    source.Filter(h =>
                    {
                        var exp = h.Empty;
                        foreach (var window in chunk.Slide(2, true))
                        {
                            var ch0 = window[0];
                            var ch1 = window[1];

                            var ch_str = ch0.ToString();
                            var str = $"{ch0}{ch1}";
                            exp |= h.Where(x => x.Simplified == ch_str) | h.Where(x => x.Simplified.Contains(str));
                        }
                        return exp.DefaultIfEmpty(h.False);
                    }),
                ];
            }
            else if (chineseType == ChineseType.Traditional)
            {
                queryResult =
                [
                    ..
                    source.Filter(h =>
                    {
                        var exp = h.Empty;
                        foreach (var window in chunk.Slide(2, true))
                        {
                            var ch0 = window[0];
                            var ch1 = window[1];

                            var ch_str = ch0.ToString();
                            var str = $"{ch0}{ch1}";
                            exp |= h.Where(x => x.Traditional == ch_str) | h.Where(x => x.Traditional.Contains(str));
                        }
                        return exp.DefaultIfEmpty(h.False);
                    }),
                ];
            }
            else throw NotSupportedChineseType(chineseType, nameof(chineseType));

            foreach (var word in queryResult)
            {
                yield return word;
            }
        }
    }

    private readonly char[] _punctuations = ['。', '，', '、', '；', '：', '？', '！', '…', '—', '·', 'ˉ', '¨', '‘', '’', '“', '”', '々', '～', '‖', '∶', '＂', '＇', '｀', '｜', '〃', '〔', '〕', '〈', '〉', '《', '》', '「', '」', '『', '』', '．', '〖', '〗', '【', '】', '（', '）', '［', '］', '｛', '｝'];

    /// <summary>
    /// 获取分词结果。
    /// </summary>
    /// <param name="chineseType"></param>
    /// <param name="sentence"></param>
    /// <returns></returns>
    public IWord[] SplitWords(string sentence, ChineseType chineseType)
    {
        var words = GetWords(chineseType, sentence).ToArray();

        var list = new LinkedList<IWord>();
        var length = sentence.Length;

        var maxOffset = Math.Min(sentence.Length, _maxWordLength);
        var ptext = length - maxOffset;
        var maxLengthPerTurn = maxOffset;
        int matchLength;

        int Resolve(string part)
        {
            var word = chineseType switch
            {
                ChineseType.Simplified => words.FirstOrDefault(x => x.Simplified == part),
                ChineseType.Traditional => words.FirstOrDefault(x => x.Traditional == part),
                _ => throw NotSupportedChineseType(chineseType, nameof(chineseType)),
            };

            if (word is not null)
            {
                list.AddFirst(word);
                return part.Length;
            }
            else
            {
                if (part.Length == 1)
                {
                    list.AddFirst(new ChineseWord
                    {
                        Simplified = part,
                        SimplifiedPinyin = null,
                        Traditional = part,
                        TraditionalPinyin = null,
                    });
                    return 1;
                }
            }

            return 0;
        }

        for (; ptext + maxLengthPerTurn >= 0; ptext -= matchLength)
        {
            matchLength = 1;
            for (var i = ptext > 0 ? 0 : -ptext; i < maxLengthPerTurn;)
            {
                var part = sentence.Substring(ptext + i, maxLengthPerTurn - i);

                if (part.Length > 1)
                {
                    var index = part.LastIndexOfAny(_punctuations);
                    if (index > -1)
                    {
                        if (index == part.Length - 1)
                        {
                            if (Resolve(part[index].ToString()) != 0) break;
                            else throw new NotImplementedException();
                        }
                        else
                        {
                            i += index + 1;
                            continue;
                        }
                    }
                }

                var _matchLength = Resolve(part);
                if (_matchLength != 0)
                {
                    matchLength = _matchLength;
                    break;
                }
                else i++;
            }
        }

        return [.. list];
    }

    /// <summary>
    /// 转换指定字符串到繁体中文。
    /// </summary>
    /// <param name="chinese"></param>
    /// <returns></returns>
    public string ToTraditional(string chinese)
    {
        var words = SplitWords(chinese, ChineseType.Simplified);
        var sb = new StringBuilder(chinese.Length * 2);

        foreach (var word in words)
        {
            sb.Append(word?.Traditional);
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
        var words = SplitWords(chinese, ChineseType.Traditional);
        var sb = new StringBuilder(chinese.Length * 2);

        foreach (var word in words)
        {
            sb.Append(word?.Simplified);
        }
        return sb.ToString();
    }

    /// <summary>
    /// 获取拼音（简体中文）
    /// </summary>
    /// <param name="chinese"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public string GetPinyin(string chinese, PinyinFormat format = PinyinFormat.Default)
    {
        return GetPinyin(chinese, ChineseType.Simplified, format);
    }

    /// <summary>
    /// 获取指定类型字符串的拼音
    /// </summary>
    /// <param name="type"></param>
    /// <param name="chinese"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    [Obsolete("Use GetPinyin(string chinese, ChineseType type, PinyinFormat format = PinyinFormat.Default) instead.")]
    public string GetPinyin(ChineseType type, string chinese, PinyinFormat format = PinyinFormat.Default)
    {
        return GetPinyin(chinese, type, format);
    }

    /// <summary>
    /// 获取指定类型字符串的拼音
    /// </summary>
    /// <param name="type"></param>
    /// <param name="chinese"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public string GetPinyin(string chinese, ChineseType type, PinyinFormat format = PinyinFormat.Default)
    {
        var words = SplitWords(chinese, type);

        if (!chinese.IsNullOrWhiteSpace())
        {
            var sb = new StringBuilder();
            var insertSpace = false;
            bool? prevLatin = null;

            foreach (var word in words)
            {
                var latin = false;
                var str = word.GetString(type);
                if (str?.Length == 1)
                {
                    var ch = str.ToCharArray()[0];
                    if ((int)ch is >= 0x21 and <= 0x7E)
                    {
                        latin = true;
                    }
                }

                var pinyin = word.GetPinyin(type);

                if (pinyin is not null)
                {
                    if ((prevLatin == true) || (insertSpace && format != PinyinFormat.InitialConsonant))
                    {
                        sb.Append(' ');
                    }

                    switch (format)
                    {
                        case PinyinFormat.Default: sb.Append(pinyin); break;
                        case PinyinFormat.WithoutTone: sb.Append(GetPinyinWithoutTone(pinyin)); break;
                        case PinyinFormat.Phonetic: sb.Append(GetPhoneticSymbol(pinyin)); break;
                        case PinyinFormat.InitialConsonant: sb.Append(GetPinyinInitialConsonant(pinyin)); break;
                    }
                    insertSpace = true;
                    prevLatin = false;
                }
                else
                {
                    if (prevLatin.HasValue && !prevLatin.Value && latin)
                    {
                        sb.Append(' ');
                    }

                    sb.Append(word.GetString(type));
                    insertSpace = false;
                    prevLatin = latin;
                }
            }

            return sb.ToString();
        }
        return chinese;
    }

    private readonly static char[] one_to_five = ['1', '2', '3', '4', '5'];

    internal static string? GetPinyinInitialConsonant(string pinyin)
    {
        var parts = pinyin.Split(' ');
        var sb = new StringBuilder(parts.Length);
        foreach (var part in parts)
        {
            sb.Append(part[0]);
        }
        return sb.ToString();
    }
    internal static string? GetPinyinWithoutTone(string pinyin)
    {
        var sb = new StringBuilder(pinyin.Length);
        foreach (var ch in pinyin)
        {
            if (!one_to_five.Contains(ch)) sb.Append(ch);
        }
        return sb.ToString();
    }
    internal static string? GetPhoneticSymbol(string pinyin)
    {
        var parts = pinyin.Split(' ').Select(part =>
        {
            if (part.Length == 1) return part;

            var _pinyin = part.Slice(0, -1);
            var tone = part[^1];

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
                "a" => tone switch { '1' => "ā", '2' => "á", '3' => "ǎ", '4' => "à", _ => "a" },
                "o" => tone switch { '1' => "ō", '2' => "ó", '3' => "ǒ", '4' => "ò", _ => "o" },
                "e" => tone switch { '1' => "ē", '2' => "é", '3' => "ě", '4' => "è", _ => "e" },
                "i" => tone switch { '1' => "ī", '2' => "í", '3' => "ǐ", '4' => "ì", _ => "i" },
                "u" => tone switch { '1' => "ū", '2' => "ú", '3' => "ǔ", '4' => "ù", _ => "u" },
                "v" => tone switch { '1' => "ǖ", '2' => "ǘ", '3' => "ǚ", '4' => "ǜ", _ => "ü" },
                _ => throw new NotImplementedException(),
            };

            return _pinyin.Replace(yunmu, _yunmu);
        });

        return parts.Join(" ");
    }

}
