namespace Modules.Inventory.Contracts.DTOs;

public record CheckStockRequestDto(Guid ProductId, int Quantity);

public record StockCheckResultDto(bool IsAvailable, int CurrentStock, string Message);
