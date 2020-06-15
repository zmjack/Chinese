using Microsoft.International.Converters.PinYinConverter;
using NStandard;
using System;
using System.Text;

namespace Chinese
{
    public static class Pinyin
    {
        public static Polyphone[] Polyphones = new Polyphone[0];

        public static string GetString(string chinese, PinyinFormat format = PinyinFormat.Default)
        {
            if (!chinese.IsNullOrWhiteSpace())
            {
                var sb = new StringBuilder();
                var insertSpace = false;
                foreach (var ch in chinese)
                {
                    try
                    {
                        var chineseChar = new ChineseChar(ch);
                        var pinyin = chineseChar.Pinyins[0].ToString().ToLower();

                        if (insertSpace) sb.Append(" ");

                        switch (format)
                        {
                            case PinyinFormat.Default: sb.Append(pinyin); break;
                            case PinyinFormat.WithoutTone: sb.Append(pinyin.Slice(0, -1)); break;
                            case PinyinFormat.PhoneticSymbol: sb.Append(GetPhoneticSymbol(pinyin)); break;
                        }
                        insertSpace = true;
                    }
                    catch
                    {
                        sb.Append(ch);
                        insertSpace = false;
                    }
                }
                return sb.ToString();
            }
            return chinese;
        }

        private static string GetPhoneticSymbol(string pinyin)
        {
            var _pinyin = pinyin.Slice(0, -1);
            var tone = int.Parse(pinyin.Slice(-1));

            string Convert(string yunmu)
            {
                var _yunmu = yunmu switch
                {
                    "a" => tone switch { 1 => "ā", 2 => "á", 3 => "ǎ", 4 => "à" },
                    "o" => tone switch { 1 => "ō", 2 => "ó", 3 => "ǒ", 4 => "ò" },
                    "e" => tone switch { 1 => "ē", 2 => "é", 3 => "ě", 4 => "è" },
                    "i" => tone switch { 1 => "ī", 2 => "í", 3 => "ǐ", 4 => "ì" },
                    "u" => tone switch { 1 => "ū", 2 => "ú", 3 => "ǔ", 4 => "ù" },
                    "v" => tone switch { 1 => "ǖ", 2 => "ǘ", 3 => "ǚ", 4 => "ǜ" },
                };

                return _pinyin.Replace(yunmu, _yunmu);
            }

            if (pinyin.Contains("v")) return Convert("v");
            else if (pinyin.Contains("iu")) return Convert("u");
            else if (pinyin.Contains("ui")) return Convert("i");
            else if (pinyin.Contains("a")) return Convert("a");
            else if (pinyin.Contains("o")) return Convert("o");
            else if (pinyin.Contains("e")) return Convert("e");
            else if (pinyin.Contains("i")) return Convert("i");
            else if (pinyin.Contains("u")) return Convert("u");
            else throw new InvalidCastException("Not a Pinyin.");
        }

    }
}
