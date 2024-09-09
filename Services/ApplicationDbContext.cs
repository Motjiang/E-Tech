using E_Tech.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Tech.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Brand> Brands { get; set; }
        //public DbSet<Cart> Carts { get; set; }
        //public DbSet<CartItem> CartItems { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }
    }
}
