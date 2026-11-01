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
            b.Property(s => s.Sku).HasMaxLength(50).IsRequired();
            b.Property(s => s.Name).HasMaxLength(200).IsRequired();
            b.Property(s => s.Price).HasPrecision(18, 2);
            b.Property(s => s.AvailableQuantity).IsRequired();
            b.Property(s => s.ReservedQuantity).IsRequired();
            b.Property(s => s.SoldQuantity).IsRequired();
        });

        modelBuilder.Entity<InventoryProcessedEvent>(b =>
        {
            b.ToTable("processed_events");
            b.HasKey(e => e.EventId);
        });
    }
}
