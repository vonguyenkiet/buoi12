using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Modules.Order.Application.DTOs;
using NetArchTest.Rules;
using Xunit;

namespace ModularMonolith.Tests;

public class IntegrationAndArchitectureTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationAndArchitectureTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Test1_DuHang_TaoDonThanhCong_TraVe201()
    {
        // Arrange: Reset dữ liệu demo về trạng thái chuẩn (Laptop: 5 chiếc)
        await _client.PostAsync("/demo/reset", null);

        var laptopId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var request = new CreateOrderRequest(
            CustomerId: Guid.NewGuid(),
            Items: new List<OrderItemRequest>
            {
                new(laptopId, 2)
            }
        );

        // Act: Đặt mua 2 chiếc Laptop
        var response = await _client.PostAsJsonAsync("/api/orders", request);

        // Assert: Kỳ vọng HTTP 201 Created, Status: Confirmed, TotalAmount: 50.000.000
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(Modules.Order.Domain.OrderStatus.Confirmed, result.Status);
        Assert.Equal(50000000m, result.TotalAmount);
    }

    [Fact]
    public async Task Test2_HetHang_TuChoiDon_TraVe409()
    {
        // Arrange: Reset dữ liệu (Chuột RGB: tồn kho = 0)
        await _client.PostAsync("/demo/reset", null);

        var mouseId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var request = new CreateOrderRequest(
            CustomerId: Guid.NewGuid(),
            Items: new List<OrderItemRequest>
            {
                new(mouseId, 1)
            }
        );

        // Act: Đặt mua 1 chiếc Chuột RGB
        var response = await _client.PostAsJsonAsync("/api/orders", request);

        // Assert: Kỳ vọng HTTP 409 Conflict, Status: Rejected
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<CreateOrderResponse>();
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(Modules.Order.Domain.OrderStatus.Rejected, result.Status);
        Assert.Contains("Tồn kho khả dụng không đủ", result.Message);
    }

    [Fact]
    public async Task Test3_DongThoiMuaSanPhamCuoi_DungMotThanhCongVaMotThatBai()
    {
        // Arrange: Reset dữ liệu (Laptop-cuối: tồn kho = 1)
        await _client.PostAsync("/demo/reset", null);

        var lastLaptopId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var request1 = new CreateOrderRequest(Guid.NewGuid(), new List<OrderItemRequest> { new(lastLaptopId, 1) });
        var request2 = new CreateOrderRequest(Guid.NewGuid(), new List<OrderItemRequest> { new(lastLaptopId, 1) });

        // Act: Gửi 2 request đồng thời song song
        var task1 = _client.PostAsJsonAsync("/api/orders", request1);
        var task2 = _client.PostAsJsonAsync("/api/orders", request2);

        var responses = await Task.WhenAll(task1, task2);

        // Assert: Chống bán vượt tồn (Anti-overselling) thành công: Đúng 1 request thành công 201 và đúng 1 request thất bại 409
        var statusCodes = responses.Select(r => r.StatusCode).ToList();
        Assert.Contains(HttpStatusCode.Created, statusCodes);
        Assert.Contains(HttpStatusCode.Conflict, statusCodes);
        Assert.Equal(1, statusCodes.Count(s => s == HttpStatusCode.Created));
        Assert.Equal(1, statusCodes.Count(s => s == HttpStatusCode.Conflict));
    }

    [Fact]
    public async Task Test4_KienTruc_OrderChiPhuThuocInterface_KhongPhuThuocNoiBoInventory()
    {
        // Arrange: Phân tích assembly của Modules.Order bằng NetArchTest
        var orderAssembly = typeof(Modules.Order.Application.OrderService).Assembly;

        // Act: Kiểm tra Order KHÔNG được phép có bất kỳ dependency nào vào mã nội bộ của Inventory
        var result = Types.InAssembly(orderAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Modules.Inventory.Domain",
                "Modules.Inventory.Infrastructure",
                "Modules.Inventory.Consumers",
                "Modules.Inventory.Application"
            )
            .GetResult();

        // Assert: Kiểm chứng ranh giới kiến trúc và tính cô lập module
        Assert.True(result.IsSuccessful, "Vi phạm ranh giới kiến trúc: Modules.Order đang phụ thuộc vào chi tiết nội bộ của Modules.Inventory!");
    }

    [Fact]
    public void Test5_KienTruc_DbContextInternal_CacModuleKhongTheTruyCapCheo()
    {
        // Arrange: Quét tất cả các kiểu DbContext trong các assembly module
        var invAssembly = typeof(Modules.Inventory.Application.InProcessInventoryService).Assembly;
        var orderAssembly = typeof(Modules.Order.Application.OrderService).Assembly;
        var notifAssembly = typeof(Modules.Notification.Consumers.NotificationOrderCreatedConsumer).Assembly;

        // Act & Assert: 100% DbContext phải là INTERNAL (NotBePublic) để ngăn chặn truy cập chéo CSDL
        var invResult = Types.InAssembly(invAssembly)
            .That().HaveNameMatching(".*DbContext")
            .Should().NotBePublic()
            .GetResult();

        var orderResult = Types.InAssembly(orderAssembly)
            .That().HaveNameMatching(".*DbContext")
            .Should().NotBePublic()
            .GetResult();

        var notifResult = Types.InAssembly(notifAssembly)
            .That().HaveNameMatching(".*DbContext")
            .Should().NotBePublic()
            .GetResult();

        Assert.True(invResult.IsSuccessful, "InventoryDbContext vi phạm: Không được khai báo public!");
        Assert.True(orderResult.IsSuccessful, "OrderDbContext vi phạm: Không được khai báo public!");
        Assert.True(notifResult.IsSuccessful, "NotificationDbContext vi phạm: Không được khai báo public!");
    }
}
