using ECBProjectCodeNoviBet.Models;
using Microsoft.EntityFrameworkCore;

namespace ECBProjectCodeNoviBet.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {}
        public DbSet<Wallet>? Wallets { get; set; }
    }
}
