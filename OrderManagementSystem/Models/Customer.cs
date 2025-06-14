namespace OrderManagementSystem.Models
{
    public enum CustomerSegment
    {
        Regular,
        Premium,
        VIP
    }
    public class Customer
    {
        public int Id { get; set; }
        public CustomerSegment Segment { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
