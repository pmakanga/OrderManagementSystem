using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.Data;
using OrderManagementSystem.DTOs;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IDiscountService _discountService;

        public OrdersController(IOrderService orderService, IDiscountService discountService)
        {
            _orderService = orderService;
            _discountService = discountService;
        }

        [HttpPost("/api/customers")]
        [SwaggerOperation(Summary = "Create a new customer")]
        public async Task<ActionResult<Customer>> CreateCustomer([FromBody] CustomerCreateDto dto)
        {
            var db = HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            if (db == null) return StatusCode(500, "DB context not found");

            var customer = new Customer
            {
                Segment = dto.Segment
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return Created($"/api/customers/{customer.Id}", customer);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new order")]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderCreateDto dto)
        {
            var db = HttpContext.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;
            if (db == null) return StatusCode(500, "DB context not found");

            var customer = await db.Customers.FindAsync(dto.CustomerId);
            if (customer == null) return NotFound("Customer not found");

            var order = new Order
            {
                CustomerId = dto.CustomerId,
                TotalAmount = dto.TotalAmount,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get order by ID")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("{id}/status")]
        [SwaggerOperation(Summary = "Update order status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromQuery] OrderStatus newStatus)
        {
            var updated = await _orderService.UpdateOrderStatusAsync(id, newStatus);
            if (!updated) return BadRequest("Invalid status transition or order not found.");
            return NoContent();
        }

        [HttpGet("{id}/discount")]
        [SwaggerOperation(Summary = "Get discount for order's customer")]
        public async Task<ActionResult<decimal>> GetDiscount(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null) return NotFound();

            var customer = HttpContext.RequestServices
                .GetService(typeof(AppDbContext)) is AppDbContext db
                ? await db.Customers.FindAsync(order.CustomerId)
                : null;

            if (customer == null) return NotFound("Customer not found");

            var discount = await _discountService.CalculateDiscountAsync(customer);
            return Ok(discount);
        }

        [HttpGet("analytics")]
        [SwaggerOperation(Summary = "Get order analytics")]
        public async Task<ActionResult<object>> GetAnalytics()
        {
            var (avgValue, avgFulfillment) = await _orderService.GetOrderAnalyticsAsync();
            return Ok(new
            {
                AverageOrderValue = avgValue,
                AverageFulfillmentTimeMinutes = avgFulfillment
            });
        }
        
    }
}
