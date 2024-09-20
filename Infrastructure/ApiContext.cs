using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Koi> Kois { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Token> Tokens { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // TblUser relationships
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Tokens)
        //        .WithOne(t => t.User)
        //        .HasForeignKey(t => t.UserId);

        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Addresses)
        //        .WithOne(a => a.User)
        //        .HasForeignKey(a => a.UserId);

        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Orders)
        //        .WithOne(o => o.User)
        //        .HasForeignKey(o => o.UserId);

        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Reviews)
        //        .WithOne(r => r.User)
        //        .HasForeignKey(r => r.UserId);

        //    // Token relationship
        //    modelBuilder.Entity<Token>()
        //        .HasOne(t => t.User)
        //        .WithMany(u => u.Tokens)
        //        .HasForeignKey(t => t.UserId);
        //    // TblOrder relationships
        //    modelBuilder.Entity<Order>()
        //        .HasMany(o => o.OrderDetails)
        //        .WithOne(od => od.Order)
        //        .HasForeignKey(od => od.OrderId);

        //    // OrderDetail relationships
        //    modelBuilder.Entity<OrderDetail>()
        //        .HasOne(od => od.Koi)
        //        .WithMany(p => p.OrderDetails)
        //        .HasForeignKey(od => od.KoiId);

        //    modelBuilder.Entity<OrderDetail>()
        //        .HasOne(od => od.Review)
        //        .WithOne(r => r.OrderDetail)
        //        .HasForeignKey<Review>(r => r.OrderDetailId);

        //    // Review relationship
        //    modelBuilder.Entity<Review>()
        //        .HasOne(r => r.OrderDetail)
        //        .WithOne(od => od.Review)
        //        .HasForeignKey<OrderDetail>(od => od.ReviewId);

        //    // Product relationships
        //    modelBuilder.Entity<Koi>()
        //        .HasMany(p => p.OrderDetails)
        //        .WithOne(od => od.Koi)
        //        .HasForeignKey(od => od.KoiId);

        //    modelBuilder.Entity<Koi>()
        //        .HasMany(p => p.Images)
        //        .WithOne(pi => pi.Koi)
        //        .HasForeignKey(pi => pi.KoiId);

        //    // ProductImage relationships
        //    modelBuilder.Entity<Image>()
        //        .HasOne(pi => pi.Koi)
        //        .WithMany(p => p.Images)
        //        .HasForeignKey(pi => pi.KoiId);

        //    // Category relationships
        //    modelBuilder.Entity<Category>()
        //        .HasMany(c => c.Kois)
        //        .WithOne(p => p.Category)
        //        .HasForeignKey(p => p.CategoryId);
        //}
    }
}
