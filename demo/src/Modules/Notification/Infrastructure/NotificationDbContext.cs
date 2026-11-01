using Microsoft.EntityFrameworkCore;
using Modules.Notification.Domain;

namespace Modules.Notification.Infrastructure;

/// <summary>
/// Infrastructure layer nội bộ của Notification Module.
/// Được khai báo INTERNAL để bảo vệ kỷ luật kiến trúc: module khác không thể truy cập DbContext này.
/// Ánh xạ độc lập vào schema "notification" trong PostgreSQL.
/// </summary>
internal class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<NotificationLog> NotificationLogs => Set<NotificationLog>();
    public DbSet<NotificationProcessedEvent> ProcessedEvents => Set<NotificationProcessedEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("notification");

        modelBuilder.Entity<NotificationLog>(b =>
        {
            b.ToTable("notification_logs");
            b.HasKey(n => n.Id);
            b.Property(n => n.Id).HasColumnName("id");
            b.Property(n => n.OrderId).HasColumnName("order_id").IsRequired();
            b.Property(n => n.EventId).HasColumnName("event_id").IsRequired();
            b.Property(n => n.Channel).HasColumnName("channel").HasMaxLength(20).IsRequired();
            b.Property(n => n.Recipient).HasColumnName("recipient").HasMaxLength(200).IsRequired();
            b.Property(n => n.Message).HasColumnName("message").HasMaxLength(1000).IsRequired();
            b.Property(n => n.IsSimulation).HasColumnName("is_simulation").IsRequired();
            b.Property(n => n.CreatedAt).HasColumnName("created_at").IsRequired();
        });

        modelBuilder.Entity<NotificationProcessedEvent>(b =>
        {
            b.ToTable("processed_events");
            b.HasKey(e => e.EventId);
            b.Property(e => e.EventId).HasColumnName("event_id");
            b.Property(e => e.ProcessedAt).HasColumnName("processed_at");
        });
    }
}
