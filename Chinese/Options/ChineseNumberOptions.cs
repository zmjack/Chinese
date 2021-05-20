namespace Chinese.Options
{
    public class ChineseNumberOptions
    {
        public bool Upper { get; set; }
        public bool Simplified { get; set; }

        public static readonly ChineseNumberOptions Default = new ChineseNumberOptions();
    }
}
