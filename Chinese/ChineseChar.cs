namespace Chinese
{
    public class ChineseChar : ChineseWord
    {
        public ChineseChar(char ch, string[] pinyins)
        {
            Char = ch;
            Pinyins = pinyins;
            IsPolyphone = Pinyins.Length > 1;
        }

        public char Char { get; }
        public string[] Pinyins { get; }
        public bool IsPolyphone { get; }
    }
}
