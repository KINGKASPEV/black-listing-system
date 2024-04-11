namespace ISWBlacklist.Domain.Entities
{
    public class Item : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public bool IsBlacklisted { get; set; } = false;
        public string Reason { get; set; }
        public string RemovalReason { get; set; }
        public DateTime? BlacklistedAt { get; set; }
        public DateTime? RemovedAt { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
