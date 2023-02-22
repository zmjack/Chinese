using Chinese.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Chinese.Data
{
    public class NumericWord : IWord
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

        public int Value { get; set; }

        public object Tag => Value;

        public bool Equals(IWord other)
        {
            return Simplified == other.Simplified && Traditional == other.Traditional;
        }
    }
}
