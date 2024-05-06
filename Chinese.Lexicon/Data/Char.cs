using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json;

namespace Chinese.Data;

[DebuggerDisplay("{Character}")]
public class Char : IWord, IEquatable<Char>
{
    [Key]
    public int Unicode { get; set; }

    public char Character { get; set; }

    [StringLength(64)]
    public string Pinyins { get; set; }

    [NotMapped]
    public string[] PinyinsProxy
    {
        get => JsonSerializer.Deserialize<string[]>(Pinyins);
        set => Pinyins = JsonSerializer.Serialize(value);
    }

    public bool IsPolyphone { get; set; }

    public ChineseType Types { get; set; }

    [StringLength(64)]
    public string Simplified { get; set; }

    [StringLength(64)]
    public string Traditional { get; set; }

    /// <summary>
    /// 默认拼音
    /// </summary>
    [StringLength(64)]
    public string DefaultPinyin { get; set; }

    public bool Sight { get; set; }

    public string SimplifiedPinyin => DefaultPinyin;
    public string TraditionalPinyin => DefaultPinyin;
    public object Tag => null;

    public bool Equals(Char other)
    {
        return Unicode == other.Unicode;
    }

    public bool Equals(IWord other)
    {
        throw new NotImplementedException();
    }
}
