using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly AppDbContext _context;


        private readonly List<DiscountRule> _rules = new()
        {
            new DiscountRule { Segment = CustomerSegment.Regular, MinOrderCount = 5, DiscountPercentage = 0.05m },
            new DiscountRule { Segment = CustomerSegment.Premium, MinOrderCount = 3, DiscountPercentage = 0.10m },
            new DiscountRule { Segment = CustomerSegment.VIP, MinOrderCount = 1, DiscountPercentage = 0.15m },
        };
        public DiscountService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<decimal> CalculateDiscountAsync(Customer customer)
        {
            var orderCount = await _context.Orders.CountAsync(o => o.CustomerId == customer.Id);

            var rule = _rules.FirstOrDefault(r =>
                r.Segment == customer.Segment &&
                orderCount >= r.MinOrderCount);

            return rule?.DiscountPercentage ?? 0;
        }
    }
}
