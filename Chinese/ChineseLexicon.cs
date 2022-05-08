#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
using Microsoft.Extensions.Caching.Memory;
#endif
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chinese
{
    public class ChineseLexicon
    {
        public int WordMaxLength { get; }
        public ChineseWord[] Words { get; }
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        public MemoryCache SimplifiedCache = new(new MemoryCacheOptions());
        public MemoryCache TraditionalCache = new(new MemoryCacheOptions());
#endif

        public ChineseLexicon(ChineseWord[] words)
        {
            Words = words;
            WordMaxLength = Words.Any() ? Words.Max(x => x.Simplified?.Length ?? 0) : 0;
        }

        public ChineseLexicon(ChineseWord[] words, int wordMaxLength)
        {
            Words = words;
            WordMaxLength = wordMaxLength;
        }

        public ChineseLexicon(params ChineseWord[][] wordsSet)
        {
            Words = GetWords(wordsSet).ToArray();
            WordMaxLength = Words.Any() ? Words.Max(x => x.Simplified?.Length ?? 0) : 0;
        }

        public ChineseLexicon(int wordMaxLength, params ChineseWord[][] wordsSet)
        {
            Words = GetWords(wordsSet).ToArray();
            WordMaxLength = wordMaxLength;
        }

        private IEnumerable<ChineseWord> GetWords(IEnumerable<ChineseWord[]> wordsSet)
        {
            foreach (var set in wordsSet)
            {
                foreach (var word in set) yield return word;
            }
        }

        /// <summary>
        /// 从词典中查询词组返回，如果未找到返回 null。
        /// </summary>
        /// <param name="type"></param>
        /// <param name="word"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        public ChineseWord Find(ChineseTypes type, string word)
        {
            if (type == ChineseTypes.Simplified)
            {
                return SimplifiedCache.GetOrCreate(string.Intern(word), entry => Words.FirstOrDefault(x => x.Simplified == word));
            }
            else if (type == ChineseTypes.Traditional)
            {
                return TraditionalCache.GetOrCreate(string.Intern(word), entry => Words.FirstOrDefault(x => x.Traditional == word));
            }
            else return null;
        }
#elif NET35_OR_GREATER
        public ChineseWord Find(ChineseTypes type, string word)
        {
            if (type == ChineseTypes.Simplified)
            {
                return Words.FirstOrDefault(x => x.Simplified == word);
            }
            else if (type == ChineseTypes.Traditional)
            {
                return Words.FirstOrDefault(x => x.Traditional == word);
            }
            else return null;
        }
#endif

        public LexiconScope BeginScope()
        {
            return new LexiconScope(this);
        }

    }
}
