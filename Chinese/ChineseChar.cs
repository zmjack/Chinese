namespace Chinese
{
    public class ChineseChar : ChineseWord
    {
        public ChineseChar(char ch, ChineseTypes types, string[] pinyins)
        {
            Char = ch;
            Pinyins = pinyins;
            IsPolyphone = Pinyins.Length > 1;
            Types = types;
        }

        public char Char { get; }
        public string[] Pinyins { get; }
        public bool IsPolyphone { get; }
        public ChineseTypes Types { get; }
    }
}
