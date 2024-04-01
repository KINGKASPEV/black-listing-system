using ISWBlacklist.Domain.Enums;

namespace ISWBlacklist.Domain.Entities
{
    public class BookCategory : BaseEntity
    {
        public string CategoryName { get; set; }
        public CategoryType Type { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}
