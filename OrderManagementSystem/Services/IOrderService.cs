using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public interface IOrderService
    {
        Task<Order?> GetOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<(decimal avgValue, double avgFulfillmentTime)> GetOrderAnalyticsAsync();
    }
}
