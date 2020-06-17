using NStandard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chinese
{
    public static class ChineseTokenizer
    {
        public static string[] SplitWords(string chinese, ChineseType chineseType = ChineseType.Simplified)
        {
            var lexicon = ChineseLexicon.Current;
            if (lexicon is null) return chinese.Select(ch => ch.ToString()).ToArray();

            var list = new LinkedList<string>();
            var length = chinese.Length;

            var maxOffset = Math.Min(chinese.Length, lexicon.WordMaxLength);
            var ptext = length - maxOffset;
            var maxLengthPerTurn = maxOffset;
            int matchLength;
            for (; ptext + maxLengthPerTurn >= 0; ptext -= matchLength)
            {
                matchLength = 1;
                for (var i = ptext > 0 ? 0 : -ptext; i < maxLengthPerTurn; i++)
                {
                    var part = chinese.Substring(ptext + i, maxLengthPerTurn - i);
                    if (part.Length == 1)
                    {
                        list.AddFirst(part);
                        break;
                    }
                    else
                    {
                        var match = Match(lexicon, chineseType, part);
                        if (match != null)
                        {
                            list.AddFirst(match);
                            matchLength = match.Length;
                            break;
                        }
                    }
                }
            }

            return list.ToArray();
        }

        private static string Match(ChineseLexicon lexicon, ChineseType chineseType, string part)
        {
            var isMatch = chineseType == ChineseType.Traditional
                ? lexicon.Words.Any(x => x.Traditional == part)
                : lexicon.Words.Any(x => x.Simplified == part);
            return isMatch ? part : null;
        }

    }
}
