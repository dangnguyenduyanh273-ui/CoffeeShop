using CoffeeShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CoffeeShop.Data
{
    public class CoffeeshopDbContext : IdentityDbContext
    {
        public CoffeeshopDbContext(DbContextOptions<CoffeeshopDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "America Coffee", Price = 25, Detail = "A bold and smooth blend from America", ImageUrl = "/images/products/america-coffee.webp", IsTrendingProduct = true },
                new Product { Id = 2, Name = "Vietnam Coffee", Price = 20, Detail = "Vietnamese coffee with a rich taste", ImageUrl = "/images/products/vietnam-coffee.webp", IsTrendingProduct = true },
                new Product { Id = 3, Name = "United Kingdom Coffee", Price = 15, Detail = "A classic UK blend", ImageUrl = "/images/products/uk-coffee.webp", IsTrendingProduct = true },
                new Product { Id = 4, Name = "India Coffee", Price = 15, Detail = "Spiced Indian coffee", ImageUrl = "/images/products/india-coffee.webp", IsTrendingProduct = false },
                new Product { Id = 5, Name = "Russian Coffee", Price = 25, Detail = "Strong Russian coffee", ImageUrl = "/images/products/russian-coffee.webp", IsTrendingProduct = false },
                new Product { Id = 6, Name = "France Coffee", Price = 35, Detail = "Elegant French coffee", ImageUrl = "/images/products/france-coffee.webp", IsTrendingProduct = false }
            );
        }
    }
}