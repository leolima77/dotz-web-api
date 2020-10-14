using Dotz.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace Dotz.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>, ApplicationUserRole, ApplicationUserLogin, IdentityRoleClaim<Guid>, ApplicationUserToken>
    {
        #region Constructor

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        #endregion Constructor

        #region ModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.EnableAutoHistory(null);
            modelBuilder.Entity<ApplicationUserRole>().HasKey(p => new { p.UserId, p.RoleId });
        }

        #endregion ModelCreating

        #region OnConfiguring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            if (!optionsBuilder.Options.Extensions.ToList().Any(a => a.GetType().Name == "MySqlOptionsExtension"))
                optionsBuilder.UseMySql(config.GetConnectionString("DefaultConnection"));
            
            optionsBuilder.UseLazyLoadingProxies();
        }

        #endregion OnConfiguring

        #region DbSets

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<AppResource> AppResource { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        #endregion DbSets
    }
}