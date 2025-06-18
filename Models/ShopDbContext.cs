using System.Data.Entity;

namespace ShopApp.Models
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext() : base("name=ShopDbContext")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .WillCascadeOnDelete(true);
        }
    }
}