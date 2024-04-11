namespace ISWBlacklist.Application.DTOs.BlacklistedItem
{
    public class BlacklistedItemResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;      
        public string Reason { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }
}
