﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Chinese.Core
{
    public interface IWord : IEquatable<IWord>
    {
        /// 简体中文字符串
        /// </summary>
        string Simplified { get; }
        /// <summary>
        /// 繁体中文字符串
        /// </summary>
        string Traditional { get; }

        /// <summary>
        /// 简体中文拼音
        /// </summary>
        string SimplifiedPinyin { get; }
        /// <summary>
        /// 繁体中文拼音
        /// </summary>
        string TraditionalPinyin { get; }

        /// <summary>
        /// 额外数据
        /// </summary>
        object Tag { get; }
    }
}
