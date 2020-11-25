using Chinese;
using DictGenerator.Generators;
using NStandard;
using System;
using System.IO;
using System.Linq;

namespace DictGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //GenerateBasic();
            //GenerateNumerical();
            GenerateGeneralWords();
        }

        static void GenerateBasic()
        {
            var gen = new BasicGenerator().Then(g => g.OnOutput += output => Console.WriteLine(output));
            var code = gen.Generate();
            File.WriteAllText("../../../Generated/Basic.cs", code);
            Console.WriteLine("File saved: Basic.cs");
        }

        static void GenerateNumerical()
        {
            var gen = new NumericalGenerator().Then(g => g.OnOutput += output => Console.WriteLine(output));
            var code = gen.Generate();
            File.WriteAllText("../../../Generated/Numerical.cs", code);
            Console.WriteLine("File saved: Numerical.cs");
        }

        static void GenerateGeneralWords()
        {
            var words = File.ReadAllText("../../../Generators/General.txt").Split("\r\n").Distinct().Where(x => x.Length > 1).ToArray();
            var content = words.Select((word, i) =>
            {
                var cwords = word.Select(ch =>
                {
                    return BuiltinWords.Basic.FirstOrDefault(x => x.Simplified == ch.ToString())
                        ?? BuiltinWords.Basic.FirstOrDefault(x => x.Traditional == ch.ToString());
                }).ToArray();
                var polyphonic = cwords?.Any(x => x?.IsPolyphone ?? false) ?? null;

                Console.WriteLine(i);

                return $"{word} ({Pinyin.GetString(word)}) [{polyphonic}]";
            });

            File.WriteAllText("../../../Generators/General-words.txt", words.Join(Environment.NewLine));
            File.WriteAllText("../../../Generators/General-all.txt", content.Join(Environment.NewLine));
            File.WriteAllText("../../../Generators/General-polyphonic.txt", content.Where(x => x.Contains("[True]")).Join(Environment.NewLine));
        }

    }
}
