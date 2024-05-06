namespace Chinese;

public enum ChineseType
{
    Undefined = 0,

    /// <summary>
    /// 简体中文
    /// </summary>
    Simplified = 1,

    /// <summary>
    /// 繁体中文
    /// </summary>
    Traditional = 2,

    /// <summary>
    /// 未简化字
    /// </summary>
    Both = Simplified | Traditional,
}
