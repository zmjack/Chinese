using System.Diagnostics;

namespace Chinese;

[DebuggerDisplay("{Char}")]
public class ChineseChar : ChineseWord
{
    public ChineseChar(char ch, ChineseType types, string[] pinyins)
    {
        Char = ch;
        Pinyins = pinyins;
        IsPolyphone = Pinyins.Length > 1;
        Types = types;
    }

    public char Char { get; }
    public string[] Pinyins { get; }
    public bool IsPolyphone { get; }
    public ChineseType Types { get; }
}
