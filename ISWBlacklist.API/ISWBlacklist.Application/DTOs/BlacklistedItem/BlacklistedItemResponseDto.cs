namespace ISWBlacklist.Application.DTOs.BlacklistedItem
{
    public class BlacklistedItemResponseDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Reason { get; set; }
        public DateTime BlacklistedAt { get; set; }
    }
}
