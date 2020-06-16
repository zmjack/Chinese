using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Chinese.Test
{
    public class ChineseConverterTests
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal("免費，跨平臺，開源！", ChineseConverter.ToTraditional("免费，跨平台，开源！"));
            Assert.Equal("免费，跨平台，开源！", ChineseConverter.ToSimplified("免費，跨平臺，開源！"));
        }

    }
}
