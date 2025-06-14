namespace OrderManagementSystem.Models
{
    public class DiscountRule
    {
        public CustomerSegment Segment { get; set; }
        public int MinOrderCount { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
