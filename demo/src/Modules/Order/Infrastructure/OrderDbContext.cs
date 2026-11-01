using Microsoft.EntityFrameworkCore;
using Modules.Order.Domain;

namespace Modules.Order.Infrastructure;

/// <summary>
/// Infrastructure layer nội bộ của Order Module.
/// Được khai báo INTERNAL để bảo vệ kỷ luật kiến trúc: module khác không thể truy cập DbContext này.
/// Ánh xạ độc lập vào schema "order" trong PostgreSQL.
/// </summary>
internal class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Domain.Order> Orders => Set<Domain.Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("order");

        modelBuilder.Entity<Domain.Order>(b =>
        {
            b.ToTable("orders");
            b.HasKey(o => o.Id);
            b.Property(o => o.Id).HasColumnName("id");
            b.Property(o => o.CustomerId).HasColumnName("customer_id").IsRequired();
            b.Property(o => o.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();
            b.Property(o => o.TotalAmount).HasColumnName("total_amount").HasPrecision(18, 2);
            b.Property(o => o.FailureReason).HasColumnName("failure_reason").HasMaxLength(500);
            b.Property(o => o.CreatedAt).HasColumnName("created_at").IsRequired();

            b.HasMany(o => o.Items)
             .WithOne()
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(b =>
        {
            b.ToTable("order_items");
            b.HasKey(i => i.Id);
            b.Property(i => i.Id).HasColumnName("id");
            b.Property(i => i.OrderId).HasColumnName("order_id").IsRequired();
            b.Property(i => i.ProductId).HasColumnName("product_id").IsRequired();
            b.Property(i => i.ProductNameSnapshot).HasColumnName("product_name_snapshot").HasMaxLength(200).IsRequired();
            b.Property(i => i.UnitPriceSnapshot).HasColumnName("unit_price_snapshot").HasPrecision(18, 2);
            b.Property(i => i.Quantity).HasColumnName("quantity").IsRequired();
        });
    }
}
