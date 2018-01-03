using Microsoft.EntityFrameworkCore;

namespace MailManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<Policy> Policies { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<Alias> Aliases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Domain>().HasKey(m => m.Id);
            builder.Entity<Domain>().ToTable("domains");

            builder.Entity<Account>().HasKey(m => m.Id);
            builder.Entity<Account>().ToTable("accounts");

            builder.Entity<Alias>().HasKey(m => m.Id);
            builder.Entity<Alias>().ToTable("aliases");

            builder.Entity<Policy>().HasKey(m => m.Id);
            builder.Entity<Policy>().ToTable("tlspolicies");
        }
    }
}
