using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        // Simple cache to optimize analytics call for 1 min
        private (DateTime fetchedAt, decimal avgValue, double avgFulfillment)? _analyticsCache;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<(decimal avgValue, double avgFulfillmentTime)> GetOrderAnalyticsAsync()
        {
            if (_analyticsCache.HasValue && (DateTime.UtcNow - _analyticsCache.Value.fetchedAt).TotalSeconds < 60)
                return (_analyticsCache.Value.avgValue, _analyticsCache.Value.avgFulfillment);

            var orders = await _context.Orders.Where(o => o.FulfilledAt.HasValue).ToListAsync();
            if (!orders.Any())
                return (0, 0);

            var avgValue = orders.Average(o => o.TotalAmount);
            var avgFulfillment = orders.Average(o => (o.FulfilledAt!.Value - o.CreatedAt).TotalMinutes);

            _analyticsCache = (DateTime.UtcNow, avgValue, avgFulfillment);

            return (avgValue, avgFulfillment);
        }

        public async Task<Order?> GetOrderAsync(int id) => 
            await _context.Orders.FindAsync(id);
        

        public async Task<IEnumerable<Order>> GetOrdersAsync() =>
            await _context.Orders.ToListAsync();


        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await GetOrderAsync(orderId);
            if (order == null) return false;

            if (!IsValidTransition(order.Status, newStatus))
                return false;

            order.Status = newStatus;
            if (newStatus == OrderStatus.Delivered)
                order.FulfilledAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        private bool IsValidTransition(OrderStatus current, OrderStatus next) =>
            current switch
            {
                OrderStatus.Pending => next == OrderStatus.Processing || next == OrderStatus.Cancelled,
                OrderStatus.Processing => next == OrderStatus.Shipped || next == OrderStatus.Cancelled,
                OrderStatus.Shipped => next == OrderStatus.Delivered,
                _ => false
            };
    }
}
