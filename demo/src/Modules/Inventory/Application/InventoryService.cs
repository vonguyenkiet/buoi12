using Modules.Inventory.Contracts;
using Modules.Inventory.Contracts.DTOs;
using Modules.Inventory.Infrastructure;

namespace Modules.Inventory.Application;

public class InventoryService : IInventoryService
{
    private readonly InventoryDbContext _dbContext;

    internal InventoryService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<StockCheckResultDto> CheckStockAsync(CheckStockRequestDto request)
    {
        var item = _dbContext.FindByProductId(request.ProductId);
        if (item == null)
        {
            return Task.FromResult(new StockCheckResultDto(false, 0, "Không tìm thấy sản phẩm trong kho."));
        }

        if (item.HasSufficientStock(request.Quantity))
        {
            return Task.FromResult(new StockCheckResultDto(true, item.AvailableQuantity, $"Đủ hàng tồn kho (Hiện có: {item.AvailableQuantity}, Yêu cầu: {request.Quantity})"));
        }

        return Task.FromResult(new StockCheckResultDto(false, item.AvailableQuantity, $"HẾT HÀNG! Tồn kho không đủ (Hiện có: {item.AvailableQuantity}, Yêu cầu: {request.Quantity})"));
    }

    public Task<bool> RestockAsync(Guid productId, int quantity)
    {
        var item = _dbContext.FindByProductId(productId);
        if (item == null) return Task.FromResult(false);

        item.Restock(quantity);
        _dbContext.Save(item);
        return Task.FromResult(true);
    }

    public Task<StockCheckResultDto> GetStockInfoAsync(Guid productId)
    {
        var item = _dbContext.FindByProductId(productId);
        if (item == null)
            return Task.FromResult(new StockCheckResultDto(false, 0, "Sản phẩm không tồn tại."));

        return Task.FromResult(new StockCheckResultDto(true, item.AvailableQuantity, $"SKU: {item.Sku} | Tên: {item.ProductName} | Tồn kho: {item.AvailableQuantity}"));
    }
}
