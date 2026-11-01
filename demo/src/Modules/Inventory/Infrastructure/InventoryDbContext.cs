using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Domain;

namespace Modules.Inventory.Infrastructure;

/// <summary>
/// Infrastructure layer nội bộ của Inventory Module.
/// Được khai báo INTERNAL để bảo vệ kỷ luật kiến trúc: module khác không thể truy cập DbContext này.
/// Ánh xạ độc lập vào schema "inventory" trong PostgreSQL.
/// </summary>
internal class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<InventoryProcessedEvent> ProcessedEvents => Set<InventoryProcessedEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("inventory");

        modelBuilder.Entity<Stock>(b =>
        {
            b.ToTable("stocks");
            b.HasKey(s => s.ProductId);
            b.Property(s => s.ProductId).HasColumnName("product_id");
            b.Property(s => s.Sku).HasColumnName("sku").HasMaxLength(50).IsRequired();
            b.Property(s => s.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            b.Property(s => s.Price).HasColumnName("price").HasPrecision(18, 2);
            b.Property(s => s.AvailableQuantity).HasColumnName("available_quantity").IsRequired();
            b.Property(s => s.ReservedQuantity).HasColumnName("reserved_quantity").IsRequired();
            b.Property(s => s.SoldQuantity).HasColumnName("sold_quantity").IsRequired();
            b.Property(s => s.CreatedAt).HasColumnName("created_at").IsRequired();
        });

        modelBuilder.Entity<InventoryProcessedEvent>(b =>
        {
            b.ToTable("processed_events");
            b.HasKey(e => e.EventId);
            b.Property(e => e.EventId).HasColumnName("event_id");
            b.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        });
    }
}
