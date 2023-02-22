using Chinese.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DbCreator.MySql
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            return ApplicationDbContext.UseMySql("server=127.0.0.1;port=33306;user=root;password=root;database=chinese", b => b.MigrationsAssembly("DbCreator.MySql"));
        }
    }
}
