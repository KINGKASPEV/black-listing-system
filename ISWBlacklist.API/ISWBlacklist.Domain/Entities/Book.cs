using System;

namespace ISWBlacklist.Domain.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string UserId { get; set; } 
        public AppUser User { get; set; }   
        public string CategoryId { get; set; } 
        public BookCategory Category { get; set; } 
        public bool IsBlacklisted { get; set; }
    }
}
