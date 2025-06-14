using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace OrderManagementSystem.Tests
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetOrder_ReturnsNotFound_ForInvalidId()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/orders/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
