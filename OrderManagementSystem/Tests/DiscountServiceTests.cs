using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;
using Xunit;

namespace OrderManagementSystem.Tests
{
    public class DiscountServiceTests
    {
        private AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;
            var context = new AppDbContext(options);

            context.Customers.Add(new Customer { Id = 1, Segment = CustomerSegment.Premium });
            context.Orders.AddRange(new[]
            {
            new Order { Id = 1, CustomerId = 1, TotalAmount = 100 },
            new Order { Id = 2, CustomerId = 1, TotalAmount = 150 },
            new Order { Id = 3, CustomerId = 1, TotalAmount = 200 }
        });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task CalculateDiscount_ReturnsCorrectDiscount()
        {
            var context = GetContext();
            var service = new DiscountService(context);

            var customer = await context.Customers.FindAsync(1);
            var discount = await service.CalculateDiscountAsync(customer!);

            Assert.Equal(0.10m, discount); // Premium customer with 3 orders
        }
    }
}
