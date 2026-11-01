using Inventory.Contracts;
using Inventory.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Inventory.Infrastructure;
using Npgsql;

namespace Modules.Inventory.Application;

public class InProcessInventoryService : IInventoryService
{
    private readonly InventoryDbContext _dbContext;
    private readonly ILogger<InProcessInventoryService> _logger;

    internal InProcessInventoryService(InventoryDbContext dbContext, ILogger<InProcessInventoryService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Giữ hàng nguyên tử (Atomic Reserve) - Chống bán vượt tồn (Anti-overselling)
    /// Sử dụng câu lệnh UPDATE với điều kiện available_quantity >= @qty trong CSDL PostgreSQL.
    /// </summary>
    public async Task<ReserveStockResult> ReserveAsync(ReserveStockRequest request, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE inventory.stocks
            SET available_quantity = available_quantity - @qty,
                reserved_quantity = reserved_quantity + @qty
            WHERE product_id = @productId AND available_quantity >= @qty
            RETURNING product_id, sku, name, price, available_quantity, reserved_quantity, sold_quantity;";

        var conn = _dbContext.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
        {
            await conn.OpenAsync(cancellationToken);
        }

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        var pId = cmd.CreateParameter();
        pId.ParameterName = "@productId";
        pId.Value = request.ProductId;
        cmd.Parameters.Add(pId);

        var pQty = cmd.CreateParameter();
        pQty.ParameterName = "@qty";
        pQty.Value = request.Quantity;
        cmd.Parameters.Add(pQty);

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var pName = reader.GetString(2);
            var uPrice = reader.GetDecimal(3);
            var avail = reader.GetInt32(4);
            var res = reader.GetInt32(5);

            _logger.LogInformation(
                "[INVENTORY] [RESERVE-SUCCESS] Giữ thành công {Quantity} sản phẩm '{Name}' (Order: {OrderId}). Còn khả dụng: {Available}, Đang giữ: {Reserved}.",
                request.Quantity, pName, request.OrderId, avail, res);

            return new ReserveStockResult(
                Success: true,
                Message: $"Đã giữ thành công {request.Quantity} sản phẩm '{pName}'.",
                AvailableQuantity: avail,
                ReservedQuantity: res,
                ProductName: pName,
                UnitPrice: uPrice
            );
        }

        await reader.CloseAsync();

        // Kiểm tra nguyên nhân thất bại: Hết hàng hay không tồn tại
        var current = await _dbContext.Stocks.AsNoTracking().FirstOrDefaultAsync(s => s.ProductId == request.ProductId, cancellationToken);
        if (current == null)
        {
            _logger.LogWarning("[INVENTORY] [RESERVE-FAILED] Sản phẩm {ProductId} không tồn tại trong kho.", request.ProductId);
            return new ReserveStockResult(false, "Sản phẩm không tồn tại trong hệ thống kho.", 0, 0);
        }

        _logger.LogWarning(
            "[INVENTORY] [RESERVE-FAILED] HẾT HÀNG! Yêu cầu {Requested}, nhưng chỉ còn khả dụng {Available} (Order: {OrderId}).",
            request.Quantity, current.AvailableQuantity, request.OrderId);

        return new ReserveStockResult(
            Success: false,
            Message: $"Tồn kho khả dụng không đủ (Hiện có: {current.AvailableQuantity}, Yêu cầu: {request.Quantity}).",
            AvailableQuantity: current.AvailableQuantity,
            ReservedQuantity: current.ReservedQuantity,
            ProductName: current.Name,
            UnitPrice: current.Price
        );
    }

    /// <summary>
    /// Nhả hàng giữ (Release) nếu quá trình lưu đơn ở Order Module bị thất bại
    /// </summary>
    public async Task<bool> ReleaseAsync(ReleaseStockRequest request, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE inventory.stocks
            SET available_quantity = available_quantity + @qty,
                reserved_quantity = reserved_quantity - @qty
            WHERE product_id = @productId AND reserved_quantity >= @qty;";

        var pId = new NpgsqlParameter("@productId", request.ProductId);
        var pQty = new NpgsqlParameter("@qty", request.Quantity);

        var rows = await _dbContext.Database.ExecuteSqlRawAsync(sql, new[] { pId, pQty }, cancellationToken);
        if (rows > 0)
        {
            _logger.LogInformation(
                "[INVENTORY] [RELEASE-SUCCESS] Đã hoàn trả {Quantity} sản phẩm về kho khả dụng (Order: {OrderId}).",
                request.Quantity, request.OrderId);
            return true;
        }

        _logger.LogWarning(
            "[INVENTORY] [RELEASE-FAILED] Không thể hoàn trả kho cho sản phẩm {ProductId} (Order: {OrderId}).",
            request.ProductId, request.OrderId);
        return false;
    }

    public async Task<StockDto?> GetStockAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var s = await _dbContext.Stocks.AsNoTracking().FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
        if (s == null) return null;

        return new StockDto(s.ProductId, s.Sku, s.Name, s.Price, s.AvailableQuantity, s.ReservedQuantity, s.SoldQuantity);
    }

    /// <summary>
    /// Trừ hàng chính thức (Confirm Sold): Chuyển reserved_quantity -> sold_quantity
    /// Được gọi bởi InventoryOrderCreatedConsumer khi sự kiện OrderCreated được xác nhận.
    /// </summary>
    public async Task<bool> ConfirmSoldAsync(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        const string sql = @"
            UPDATE inventory.stocks
            SET reserved_quantity = reserved_quantity - @qty,
                sold_quantity = sold_quantity + @qty
            WHERE product_id = @productId AND reserved_quantity >= @qty;";

        var pId = new NpgsqlParameter("@productId", productId);
        var pQty = new NpgsqlParameter("@qty", quantity);

        var rows = await _dbContext.Database.ExecuteSqlRawAsync(sql, new[] { pId, pQty }, cancellationToken);
        return rows > 0;
    }
}
