using Chinese.Core;
using Chinese.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinese.Data
{
    public class Word : IWord
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 词组类型
        /// </summary>
        public WordType Type { get; set; }

        /// <summary>
        /// 简体中文字符串
        /// </summary>
        [StringLength(64)]
        public string Simplified { get; set; }

        /// <summary>
        /// 繁体中文字符串
        /// </summary>
        [StringLength(64)]
        public string Traditional { get; set; }

        /// <summary>
        /// 简体中文拼音
        /// </summary>
        [StringLength(64)]
        public string SimplifiedPinyin { get; set; }

        /// <summary>
        /// 繁体中文拼音
        /// </summary>
        [StringLength(64)]
        public string TraditionalPinyin { get; set; }

        public int HashCode { get; set; }

        public object Tag => null;

        public bool Equals(IWord other)
        {
            return Simplified == other.Simplified && Traditional == other.Traditional;
        }
    }
}
