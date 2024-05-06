using Chinese.Data;
using Microsoft.EntityFrameworkCore;

namespace DbCreator;

class Program
{
    static void Main(string[] args)
    {
        var sqliteOptions = new DbContextOptionsBuilder<ChineseDbContext>().UseSqlite("filename=chinese.db", null).Options;
        var mysqlConnectionString = "server=127.0.0.1;database=chinese";
        var mysqlOptions = new DbContextOptionsBuilder<ChineseDbContext>().UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString), null).Options;

        using (var sqlite = new ChineseDbContext(sqliteOptions))
        using (var mysql = new ChineseDbContext(mysqlOptions))
        {
            sqlite.Database.Migrate();
        }

        Console.WriteLine("Complete.");
    }
}
