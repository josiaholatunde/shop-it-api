using Microsoft.EntityFrameworkCore;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BrandCategory> BrandCategory { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BrandCategory>()
                        .HasKey(p => new {p.CategoryId, p.BrandId});

            modelBuilder.Entity<BrandCategory>()
                        .HasOne<Category>(p => p.Category)
                        .WithMany(p => p.Brands)
                        .HasForeignKey(p => p.CategoryId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BrandCategory>()
                        .HasOne<Brand>(p => p.Brand)
                        .WithMany(p => p.Category)
                        .HasForeignKey(p => p.BrandId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}