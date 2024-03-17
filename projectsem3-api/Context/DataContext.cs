using Microsoft.EntityFrameworkCore;
using projectsem3_api.Entities;

namespace projectsem3_api.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Restaurant> Restaurants { get; set;}
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<FoodType> FoodTypes { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboDetail> ComboDetails { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartsDetail { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}
