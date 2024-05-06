using NStandard;
using System;
using System.Linq;
using System.Text;

namespace Chinese.Lexicons;

public class NumberLexicon : Lexicon
{
    private const int PART_COUNT = 4;
    private const int SUPPORTED_SEPARATOR_COUNT = 8;
    private string[] _separators;
    /// <summary>
    /// 数值分级单位
    /// </summary>
    public string[] Separators
    {
        get => _separators;
        set
        {
            if (value.Length > SUPPORTED_SEPARATOR_COUNT) throw new NotSupportedException($"超过最大支持分隔符数量（{SUPPORTED_SEPARATOR_COUNT}）。");
            _separators = value;
        }
    }
    public bool Code { get; set; }
    public bool Concise { get; set; }
    public ChineseType ChineseType { get; set; }

    private readonly string[] _values;
    private readonly string[] _levels;

    public NumberLexicon(NumberMode mode)
    {
        Add(Builtin.NumericalWords);

        Code = mode.HasFlag(NumberMode.Code);
        Concise = mode.HasFlag(NumberMode.Concise);
        ChineseType = mode.HasFlag(NumberMode.Traditional) ? ChineseType.Traditional : ChineseType.Simplified;

        if (mode.HasFlag(NumberMode.Classical))
        {
            Separators = [string.Empty, "万", "亿", "兆", "京", "垓", "秭", "穰"];
        }
        else
        {
            Separators = [string.Empty, "万", "亿", "万亿", "亿亿", "万亿亿", "亿亿亿", "万亿亿亿"];
        }

        if (mode.HasFlag(NumberMode.Upper))
        {
            _levels = [string.Empty, "拾", "佰", "仟"];
            _values = ["零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"];
        }
        else
        {
            _levels = [string.Empty, "十", "百", "千"];
            _values =
            [
                mode.HasFlag(NumberMode.Code) ? "〇" : "零",
                "一", "二", "三", "四", "五", "六", "七", "八", "九"
            ];
        }
    }

    /// <summary>
    /// 获取数字的编号读法。
    /// </summary>
    /// <param name="s_number"></param>
    /// <returns></returns>
    private string GetCodeString(decimal number)
    {
        var s_number = number.ToString();
        if (!s_number.All(ch => '0' <= ch && ch <= '9')) throw new ArgumentException("不是合法的数字编号。");

        var sb = new StringBuilder(s_number.Length + 1);
        foreach (var ch in s_number) sb.Append(_values[ch - '0']);

        return sb.ToString();
    }

    /// <summary>
    /// 获取数字的编号读法。
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private decimal GetCodeNumber(string number)
    {
        var sb = new StringBuilder(number.Length + 1);
        foreach (var ch in number)
        {
            var value = _values.IndexOf(ch.ToString());

            if (value > -1) sb.Append(value);
            else throw new ArgumentException("不是合法的中文数字编号描述。", nameof(number));
        }

        return decimal.Parse(sb.ToString());
    }

    /// <summary>
    /// 获取数值的数值读法。
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public string GetString(decimal number)
    {
        if (Code) return GetCodeString(number);

        number = number switch
        {
            0 => number,
            > 0 => decimal.Floor(number),
            < 0 => decimal.Ceiling(number),
        };

        string GetPartString(char[] singles, string level, bool prevZero)
        {
            if (!singles.Any()) return string.Empty;

            var sb = new StringBuilder();
            var zero = prevZero;
            foreach (var (index, single) in singles.Pairs())
            {
                if (single != '0')
                {
                    var value = _values[single - '0'];
                    var unit = _levels[singles.Length - 1 - index];

                    if (zero) sb.Append(_values[0]);
                    sb.Append($"{value}{unit}");

                    zero = false;
                }
                else
                {
                    zero = true;
                }
            }

            if (sb.Length == 0) return null;
            else
            {
                sb.Append(level);
                return sb.ToString();
            }
        }

        var s_number = number.ToString();
        var enumerator_s_number = s_number.GetEnumerator();

        var levelParts = new char[(s_number.Length - 1) / 4 + 1][];
        var enumerator_levelParts = levelParts.GetEnumerator();
        if (enumerator_levelParts.MoveNext())
        {
            var mod = s_number.Length % PART_COUNT;
            levelParts[0] = new char[mod > 0 ? mod : 4];
            for (int j = 0; j < levelParts[0].Length; j++)
            {
                enumerator_s_number.MoveNext();
                levelParts[0][j] = enumerator_s_number.Current;
            }

            int i = 1;
            while (enumerator_levelParts.MoveNext())
            {
                levelParts[i] = new char[4];
                for (int j = 0; j < PART_COUNT; j++)
                {
                    enumerator_s_number.MoveNext();
                    levelParts[i][j] = enumerator_s_number.Current;
                }
                i++;
            }
        }

        var sb = new StringBuilder();
        int part_i = 0;
        bool prevZero = false;
        foreach (var part in levelParts)
        {
            var partString = GetPartString(part, Separators[levelParts.Length - 1 - part_i], prevZero);
            if (partString is not null)
            {
                sb.Append(partString);
                prevZero = false;
            }
            else prevZero = true;
            part_i++;
        }

        if (sb.Length == 0) return _values[0];

        if (Concise && sb.Length > 1)
        {
            if (sb[0] == '一' && sb[1] == '十' || sb[0] == '壹' && sb[1] == '拾')
            {
                sb.Remove(0, 1);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 获取数值读法的数值。
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public decimal GetNumber(string number)
    {
        if (Code) return GetCodeNumber(number);

        if (number.Length == 0) return default;

        var last = number.Last();
        if (last == '两') throw new ArgumentException($"不能以该字结尾：{last}", nameof(number));

        var words = SplitWords(number, ChineseType);
        var total = 0m;
        var levelNumber = 0;
        foreach (var word in words)
        {
            var ch = word.Simplified;

            if (ch == "零") continue;
            else if (ch == "十" || ch == "拾") levelNumber += 10;
            else
            {
                if (word.Tag is int @int)
                {
                    levelNumber += @int;
                }
                else
                {
                    var level = _separators.IndexOf(ch);
                    if (level > -1)
                    {
                        total += levelNumber * level switch
                        {
                            1 => 1_0000,
                            2 => 1_0000_0000,
                            3 => 1_0000_0000_0000,
                            4 => 1_0000_0000_0000_0000,
                            5 => 1_0000_0000_0000_0000_0000m,
                            6 => 1_0000_0000_0000_0000_0000_0000m,
                            7 => 1_0000_0000_0000_0000_0000_0000_0000m,
                            _ => throw new NotImplementedException(),
                        };
                        levelNumber = 0;
                    }
                    else throw new ArgumentException($"不能解析的词汇：{word}", nameof(number));
                }
            }
        }
        total += levelNumber;
        return total;
    }

}
