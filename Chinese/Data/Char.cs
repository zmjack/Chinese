using Chinese.Core;
using Chinese.Data;
using LinqSharp.EFCore.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Chinese.Data
{
    public class Char : IWord, IEquatable<Char>
    {
        [Key]
        public int Unicode { get; set; }

        public char Character { get; set; }

        [JsonProvider]
        [StringLength(64)]
        public string[] Pinyins { get; set; }

        public bool IsPolyphone { get; set; }

        public ChineseType Types { get; set; }

        [StringLength(64)]
        public string Simplified { get; set; }

        [StringLength(64)]
        public string Traditional { get; set; }

        /// <summary>
        /// 默认拼音
        /// </summary>
        [StringLength(64)]
        public string DefaultPinyin { get; set; }

        public bool Sight { get; set; }

        public string SimplifiedPinyin => DefaultPinyin;
        public string TraditionalPinyin => DefaultPinyin;
        public object Tag => null;

        public bool Equals(Char other)
        {
            return Unicode == other.Unicode;
        }

        public bool Equals(IWord other)
        {
            throw new NotImplementedException();
        }
    }
}
