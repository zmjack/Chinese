using Microsoft.EntityFrameworkCore;

namespace Chinese.Data;

public class ChineseDbContext : DbContext
{
    public ChineseDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Char> Chars { get; set; }
    public DbSet<Word> Words { get; set; }

}
