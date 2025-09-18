using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace WebApi_II.Models
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set;}
        public DbSet<Product> products { get; set;}
        public DbSet<ItemOrder> ItemsOrder { get; set;}
        public OrderContext(DbContextOptions<OrderContext> options):base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
