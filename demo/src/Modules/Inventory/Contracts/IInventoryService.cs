using Modules.Inventory.Contracts.DTOs;

namespace Modules.Inventory.Contracts;

/// <summary>
/// Public Service Interface (Architectural Seam):
/// Cửa ngõ công khai DUY NHẤT để các module bên ngoài (Order Module) giao tiếp với Inventory.
/// Khi tiến hóa lên Microservices, Order Service giữ nguyên 100% logic nghiệp vụ,
/// chỉ thay implementation nội bộ bằng gRPC/REST Client.
/// </summary>
public interface IInventoryService
{
    Task<StockCheckResultDto> CheckStockAsync(CheckStockRequestDto request);
    Task<bool> RestockAsync(Guid productId, int quantity);
    Task<StockCheckResultDto> GetStockInfoAsync(Guid productId);
}
