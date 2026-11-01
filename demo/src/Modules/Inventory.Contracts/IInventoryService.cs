using Inventory.Contracts.DTOs;

namespace Inventory.Contracts;

/// <summary>
/// Public Service Interface (Architectural Seam):
/// Hợp đồng dịch vụ công khai DUY NHẤT để Order Module giao tiếp đồng bộ với Inventory Module.
/// Không để lộ Domain Entity nội bộ của Inventory ra ngoài.
/// OrderService phụ thuộc vào interface này và không thay đổi bất kỳ dòng code nào
/// khi chuyển đổi giữa chế độ InProcess (gọi RAM) và Http (REST qua loopback hoặc sang service ngoài).
/// </summary>
public interface IInventoryService
{
    Task<ReserveStockResult> ReserveAsync(ReserveStockRequest request, CancellationToken cancellationToken = default);
    Task<bool> ReleaseAsync(ReleaseStockRequest request, CancellationToken cancellationToken = default);
    Task<StockDto?> GetStockAsync(Guid productId, CancellationToken cancellationToken = default);
}
