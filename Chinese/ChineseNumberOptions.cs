namespace Chinese
{
    public class ChineseNumberOptions
    {
        public bool Upper { get; set; }
        public bool Verbose { get; set; }

        public static readonly ChineseNumberOptions Default = new ChineseNumberOptions();
    }
}
