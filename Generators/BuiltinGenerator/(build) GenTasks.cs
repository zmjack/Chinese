using BuiltinGenerator.Generators;
using Chinese;
using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace BuiltinGenerator
{
    public class GenTasks
    {
        [Fact]
        public void GenerateChineseChars()
        {
            var gen = new ChineseCharsGenerator();
            gen.OnOutput += output => Debug.WriteLine(output);
            var code = gen.Generate();
            File.WriteAllText("../../../Codes/Builtin/ChineseChars.cs", code);
        }

        [Fact]
        public void GenerateNumericalWords()
        {
            var valid = Builtin.ChineseChars?.Length > 0;
            if (!valid) throw new InvalidOperationException("Builtin.ChineseChars is empty.");

            var gen = new NumericalGenerator();
            gen.OnOutput += output => Debug.WriteLine(output);
            var code = gen.Generate();
            File.WriteAllText("../../../Codes/Builtin/NumericalWords.cs", code);
        }
    }
}
