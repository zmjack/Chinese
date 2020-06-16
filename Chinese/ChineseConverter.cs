using System;
using System.Collections.Generic;
using System.Text;

namespace Chinese
{
    using Converter = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConverter;
    using Direction = Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConversionDirection;

    public static class ChineseConverter
    {
        public static string ToTraditional(string chinese) => Converter.Convert(chinese, Direction.SimplifiedToTraditional);
        public static string ToSimplified(string chinese) => Converter.Convert(chinese, Direction.TraditionalToSimplified);
    }
}
