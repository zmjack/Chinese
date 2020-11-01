namespace Chinese
{
    public class ChineseWord
    {
        public string[] Pinyins { get; set; }
        public bool IsPolyphone { get; }
        public string Simplified { get; set; }
        public string Traditional { get; set; }
        public object Tag { get; set; }
    }
}
