using NStandard;
using System;
using System.Text.RegularExpressions;

namespace Chinese.Lexicons;

public class CurrencyLexicon : Lexicon
{
    private readonly NumberLexicon _numericLexicon;

    private readonly Regex _currencyRegex;
    private readonly string[] _levels;

    public CurrencyLexicon(NumberMode mode)
    {
        _numericLexicon = new(mode);

        if (mode.HasFlag(NumberMode.Upper))
        {
            _levels = ["圆", "角", "分"];
            _currencyRegex = new Regex(@"^(.+)(?:圆)(?:整|(.)角整|(.)角(.)分|零(.)分)$", RegexOptions.Compiled);
        }
        else
        {
            _levels = ["元", "角", "分"];
            _currencyRegex = new Regex(@"^(.+)(?:元)(?:整|(.)角整|(.)角(.)分|零(.)分)$", RegexOptions.Compiled);
        }
    }

    /// <summary>
    /// 获取数值的货币读法。
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="upper"></param>
    /// <returns></returns>
    public string GetString(decimal currency)
    {
        var fractional100 = (int)(currency % 1 * 100 % 100);

        var yuan = $"{_numericLexicon.GetString(currency)}{_levels[0]}";
        if (fractional100 == 0)
        {
            return $"{yuan}整";
        }
        else if (fractional100 % 10 == 0)
        {
            var jiao = $"{_numericLexicon.GetString(fractional100 / 10)}{_levels[1]}";
            return $"{yuan}{jiao}整";
        }
        else
        {
            var jiao_value = fractional100 / 10;
            var jiao = jiao_value > 0
                ? $"{_numericLexicon.GetString(fractional100 / 10)}{_levels[1]}"
                : "零";

            var fen = $"{_numericLexicon.GetString(fractional100 % 10)}{_levels[2]}";
            return $"{yuan}{jiao}{fen}";
        }
    }

    /// <summary>
    /// 获取货币读法的数值。
    /// </summary>
    /// <param name="currency"></param>
    /// <returns></returns>
    public decimal GetNumber(string currency)
    {
        var match = _currencyRegex.Match(currency);

        if (!match.Success) throw new ArgumentException("不是合法的中文货币描述。", nameof(currency));

        var yuan = _numericLexicon.GetNumber(match.Groups[1].Value);

        var jiao_value = match.Groups[2].Value;
        if (jiao_value.IsWhiteSpace()) jiao_value = match.Groups[3].Value;
        var jiao = jiao_value.IsWhiteSpace() ? 0m : _numericLexicon.GetNumber(jiao_value);

        var fen_value = match.Groups[4].Value;
        if (fen_value.IsWhiteSpace()) fen_value = match.Groups[5].Value;
        var fen = fen_value.IsWhiteSpace() ? 0m : _numericLexicon.GetNumber(fen_value);

        return yuan + jiao * 0.1m + fen * 0.01m;
    }

}
