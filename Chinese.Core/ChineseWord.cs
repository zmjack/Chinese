using Chinese.Core;
using System;

namespace Chinese
{
    public class ChineseWord : IWord
    {
        /// <summary>
        /// 中文字符串
        /// </summary>
        public string Word
        {
            set
            {
                Simplified = value;
                Traditional = value;
            }
        }
        /// <summary>
        /// 简体中文字符串
        /// </summary>
        public string Simplified { get; set; }
        /// <summary>
        /// 繁体中文字符串
        /// </summary>
        public string Traditional { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin
        {
            set
            {
                SimplifiedPinyin = value;
                TraditionalPinyin = value;
            }
        }
        /// <summary>
        /// 简体中文拼音
        /// </summary>
        public string SimplifiedPinyin { get; set; }
        /// <summary>
        /// 繁体中文拼音
        /// </summary>
        public string TraditionalPinyin { get; set; }

        /// <summary>
        /// 额外数据
        /// </summary>
        public object Tag { get; set; }

        public bool Equals(IWord other)
        {
            return Simplified == other.Simplified && Traditional == other.Traditional;
        }
    }
}
