namespace OrderManagementSystem.DTOs
{
    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
