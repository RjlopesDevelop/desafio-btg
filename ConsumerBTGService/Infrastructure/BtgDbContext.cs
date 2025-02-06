using ConsumerBTGService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsumerBTGService.Infrastructure
{
    public class BtgDbContext : DbContext
    {
        public BtgDbContext(DbContextOptions<BtgDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(Settings.GetStringConnection(), new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");
                entity.HasKey(o => o.OrderId).HasName("Order_Id");
                entity.HasMany(o => o.OrderDetails).WithOne(d => d.Order).HasForeignKey(d => d.OrderId);
                entity.Property(o => o.OrderId).HasColumnName("Order_Id");
                entity.Property(o => o.OrderDate).HasColumnName("Order_Date");
                entity.Property(o => o.CustomerId).HasColumnName("Customer_Id");
                entity.Property(o => o.TotalAmount).HasColumnName("Total_Amount");

            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("order_detail");
                entity.HasKey(d => d.OrderDetailId).HasName("Id");
                entity.Property(d => d.OrderDetailId).HasColumnName("Id");
                entity.Property(d => d.OrderId).HasColumnName("Order_Id");
                entity.Property(d => d.ProductName).HasColumnName("Product_Name");
                entity.Property(d => d.Quantity).HasColumnName("Quantity");
                entity.Property(d => d.UnitPrice).HasColumnName("Unit_Price");
            });

        }
    }
}