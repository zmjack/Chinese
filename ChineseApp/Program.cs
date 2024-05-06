using Chinese;
using Chinese.Data;
using Chinese.Lexicons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ChineseApp;

internal class Program
{
    public class MySqlLexicon : Lexicon
    {
        public static MySqlLexicon UseDefault()
        {
            var mysqlConnectionString = "server=127.0.0.1;port=33306;user=root;password=root;database=chinese";
            var mysqlOptions = new DbContextOptionsBuilder<ChineseDbContext>()
                .UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString), null)
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())).Options;

            var context = new ChineseDbContext(mysqlOptions);
            return new MySqlLexicon(context);
        }

        public MySqlLexicon(ChineseDbContext context)
        {
            Add(context.Words);
            Add(context.Chars);
        }
    }

    static void Main(string[] args)
    {
        var watch = new Stopwatch();

        //var lexicon = MySqlLexicon.UseDefault();
        //Console.WriteLine(lexicon.GetPinyin("只有这只鸟跑得很快。"));

        {
            var lexicon = Lexicon.Currency;
            Console.WriteLine(
                Lexicon.Currency.GetString(0.1m)
                );
        }

        {
            watch.Restart();
            var currency = Lexicon.Currency;
            for (int i = 0; i < 100; i++)
            {
                var result = currency.GetNumber("一百二十三亿亿亿四千五百六十七万亿亿八千九百零一亿亿二千三百四十五万亿六千七百八十九亿零一百二十三万四千五百六十七元八角九分");
                if (i == 0) Console.WriteLine(result);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);

            watch.Restart();
            for (int i = 0; i < 100; i++)
            {
                var result = currency.GetString(123456789012345678901234567.89M);
                if (i == 0) Console.WriteLine(result);
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }
    }
}
