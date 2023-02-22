using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LinqSharp.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Chinese.Data
{
    public class ApplicationDbContext : DbContext
    {
        public static ApplicationDbContext UseSqlite(string connectionString, Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null)
        {
            return new(new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connectionString, sqliteOptionsAction).Options);
        }
        public static ApplicationDbContext UseMySql(string connectionString, Action<MySqlDbContextOptionsBuilder> sqliteOptionsAction = null)
        {
            return new(new DbContextOptionsBuilder<ApplicationDbContext>().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), sqliteOptionsAction).Options);
        }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Char> Chars { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<NumericWord> Numerics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            LinqSharpEF.OnModelCreating(this, modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return LinqSharpEF.SaveChanges(this, base.SaveChanges, acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return LinqSharpEF.SaveChangesAsync(this, base.SaveChangesAsync, acceptAllChangesOnSuccess, cancellationToken);
        }

    }
}
