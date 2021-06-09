using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Crayon.Api.Sdk.Domain;
using Crayon.Api.Sdk.Domain.Csp;
using Crayon.Api.Sdk.Domain.MasterData;
using Microsoft.Extensions.Logging;

namespace Crayon_API.Data
{
    public class CrayonDbContext : DbContext
    {
        private const string connectionString = @"Connection String";

        public CrayonDbContext() : base()
        {

        }

        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
               category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Debug).AddConsole();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString).UseLoggerFactory(loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<BillingStatement> BillingStatements { get; set; }
        //public DbSet<Publisher> Publishers { get; set; }
        //public DbSet<InvoiceProfile> Invoices { get; set; }
        //public DbSet<AgreementProduct> Products { get; set; }
        //public DbSet<Agreement> Agreements { get; set; }
        //public DbSet<BlogItem> BlogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AddressData>().HasNoKey();
            //modelBuilder.Entity<Price>().HasNoKey();

            modelBuilder.Entity<BillingStatement>().OwnsOne<Price>(s => s.TotalSalesPrice);
            modelBuilder.Entity<BillingStatement>().OwnsOne<ObjectReference>(s => s.InvoiceProfile);
            modelBuilder.Entity<BillingStatement>().OwnsOne<ObjectReference>(s => s.Organization);
            modelBuilder.Entity<BillingStatement>().Property(b => b.Id).ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
