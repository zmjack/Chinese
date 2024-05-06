using Chinese.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DbCreator.MySql;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ChineseDbContext>
{
    public ChineseDbContext CreateDbContext(string[] args)
    {
        var mysqlConnectionString = "server=127.0.0.1;port=33306;user=root;password=root;database=chinese";
        var mysqlOptions = new DbContextOptionsBuilder<ChineseDbContext>().UseMySql(
            mysqlConnectionString,
            ServerVersion.AutoDetect(mysqlConnectionString),
            b => b.MigrationsAssembly("DbCreator.MySql")
        ).Options;

        return new ChineseDbContext(mysqlOptions);
    }
}
