using System.Collections.Concurrent;
using System.Text.Json;
using Modules.Notification.Domain;

namespace Modules.Notification.Infrastructure;

/// <summary>
/// Infrastructure Layer nội bộ của Notification Module.
/// Được đánh dấu INTERNAL để đảm bảo nguyên tắc cô lập dữ liệu.
/// Dữ liệu thông báo lưu độc lập trong data/notification/notification_schema.json.
/// </summary>
internal class NotificationDbContext
{
    private readonly string _storagePath;
    private readonly ConcurrentBag<NotificationLog> _logs = new();
    private readonly object _lock = new();

    public NotificationDbContext(string baseDataDir = "data/notification")
    {
        Directory.CreateDirectory(baseDataDir);
        _storagePath = Path.Combine(baseDataDir, "notification_schema.json");
        LoadFromDisk();
    }

    private void LoadFromDisk()
    {
        lock (_lock)
        {
            if (File.Exists(_storagePath))
            {
                var json = File.ReadAllText(_storagePath);
                var items = JsonSerializer.Deserialize<List<NotificationLogRecord>>(json);
                if (items != null)
                {
                    foreach (var it in items)
                    {
                        _logs.Add(new NotificationLog(it.OrderId, it.Channel, it.Recipient, it.Message));
                    }
                }
            }
        }
    }

    private void SaveToDisk()
    {
        lock (_lock)
        {
            var records = _logs.Select(l => new NotificationLogRecord(l.Id, l.OrderId, l.Channel, l.Recipient, l.Message, l.SentAt)).ToList();
            var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_storagePath, json);
        }
    }

    public void AddLog(NotificationLog log)
    {
        _logs.Add(log);
        SaveToDisk();
    }

    public IEnumerable<NotificationLog> GetAll() => _logs;

    private record NotificationLogRecord(Guid Id, Guid OrderId, string Channel, string Recipient, string Message, DateTime SentAt);
}
