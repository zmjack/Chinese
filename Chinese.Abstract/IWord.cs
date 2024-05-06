namespace Chinese;

public interface IWord : IEquatable<IWord>
{
    /// 简体中文字符串
    /// </summary>
    string? Simplified { get; }
    /// <summary>
    /// 繁体中文字符串
    /// </summary>
    string? Traditional { get; }

    /// <summary>
    /// 简体中文拼音
    /// </summary>
    string? SimplifiedPinyin { get; }
    /// <summary>
    /// 繁体中文拼音
    /// </summary>
    string? TraditionalPinyin { get; }

    string? GetString(ChineseType type)
    {
        if (type == ChineseType.Simplified) return Simplified;
        else if (type == ChineseType.Traditional) return Traditional;
        else throw new NotSupportedException($"The type is not supported. (type: {type})");
    }

    string? GetPinyin(ChineseType type)
    {
        if (type == ChineseType.Simplified) return SimplifiedPinyin;
        else if (type == ChineseType.Traditional) return TraditionalPinyin;
        else throw new NotSupportedException($"The type is not supported. (type: {type})");
    }

    /// <summary>
    /// 额外数据
    /// </summary>
    object? Tag { get; }
}
