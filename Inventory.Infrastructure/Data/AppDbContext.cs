using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Model;

namespace Inventory.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductClassification> ProductClassifications { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<ProductTax> ProductTaxes { get; set; }
        public DbSet<PurchaseItemTax> PurchaseItemTaxes { get; set; }
        public DbSet<InventoryStock> InventoryStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductClassification>().HasData(
                new ProductClassification { Id = 1, Name = "Electronics", Code = "ELEC", ParentClassificationId = null },
                new ProductClassification { Id = 2, Name = "Mobile Phones", Code = "MOB", ParentClassificationId = 1 },
                new ProductClassification { Id = 3, Name = "Laptops", Code = "LAP", ParentClassificationId = 1 },
                new ProductClassification { Id = 4, Name = "Groceries", Code = "GROC", ParentClassificationId = null },
                new ProductClassification { Id = 5, Name = "Beverages", Code = "BEV", ParentClassificationId = 4 }
            );

            // Product
            modelBuilder.Entity<Product>()
                .HasIndex(x => x.SKU)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .Property(x => x.PurchasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(x => x.SellingPrice)
                .HasPrecision(18, 2);

            // Tax
            modelBuilder.Entity<Tax>()
                .Property(x => x.Percentage)
                .HasPrecision(5, 2);

            // ProductTax (Many-to-Many via Join Entity)
            modelBuilder.Entity<ProductTax>()
                .HasKey(pt => new { pt.ProductId, pt.TaxId });

            modelBuilder.Entity<ProductTax>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTaxes)
                .HasForeignKey(pt => pt.ProductId);

            modelBuilder.Entity<ProductTax>()
                .HasOne(pt => pt.Tax)
                .WithMany(t => t.ProductTaxes)
                .HasForeignKey(pt => pt.TaxId);

            // Purchase
            modelBuilder.Entity<Purchase>()
                .HasOne(x => x.Supplier)
                .WithMany(x => x.Purchases)
                .HasForeignKey(x => x.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Purchase>()
                .Property(x => x.SubTotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Purchase>()
                .Property(x => x.TaxAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Purchase>()
                .Property(x => x.GrandTotal)
                .HasPrecision(18, 2);

            // Purchase Item
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(x => x.Purchase)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseId);

            modelBuilder.Entity<PurchaseItem>()
                .HasOne(x => x.Product)
                .WithMany(x => x.PurchaseItems)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PurchaseItem>()
                .Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseItem>()
                .Property(x => x.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseItem>()
                .Property(x => x.TaxAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PurchaseItem>()
                .Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            // Purchase Item Tax
            modelBuilder.Entity<PurchaseItemTax>()
                .HasOne(x => x.PurchaseItem)
                .WithMany(x => x.Taxes)
                .HasForeignKey(x => x.PurchaseItemId);

            modelBuilder.Entity<PurchaseItemTax>()
                .Property(x => x.TaxPercentage)
                .HasPrecision(5, 2);

            modelBuilder.Entity<PurchaseItemTax>()
                .Property(x => x.TaxAmount)
                .HasPrecision(18, 2);
        }
    }
}
    