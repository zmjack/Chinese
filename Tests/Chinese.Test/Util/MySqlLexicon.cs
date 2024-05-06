using Chinese.Data;
using Microsoft.EntityFrameworkCore;

namespace Chinese.Test.Util;

public class MySqlLexicon : Lexicon
{
    public static MySqlLexicon UseDefault()
    {
        var mysqlConnectionString = "server=127.0.0.1;port=33306;user=root;password=root;database=chinese";
        var mysqlOptions = new DbContextOptionsBuilder<ChineseDbContext>().UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString), null).Options;

        var context = new ChineseDbContext(mysqlOptions);
        return new MySqlLexicon(context);
    }

    public MySqlLexicon(ChineseDbContext context)
    {
        Add(context.Words);
        Add(context.Chars);
    }
}
