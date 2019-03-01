using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace MailManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<TlsPolicy> TlsPolicies { get; set; }

        public DbSet<Account> Accounts { get; set; }
        
        public DbSet<Alias> Aliases { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Domain>().HasKey(d => d.Id);
            builder.Entity<Domain>().ToTable("domains");

            builder.Entity<Account>().HasKey(a => a.Id);
            builder.Entity<Account>().Property(a => a.Enabled).HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Entity<Account>().Property(a => a.Sendonly).HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Entity<Account>().ToTable("accounts");

            builder.Entity<Alias>().HasKey(a => a.Id);
            builder.Entity<Alias>().Property(a => a.Enabled).HasConversion(new BoolToZeroOneConverter<Int16>());
            builder.Entity<Alias>().ToTable("aliases");

            builder.Entity<TlsPolicy>().HasKey(t => t.Id);
            builder.Entity<TlsPolicy>().ToTable("tlspolicies");
        }
    }
}
