using System.ComponentModel.DataAnnotations;

namespace ISWBlacklist.Application.DTOs.Item
{
    public class ItemCreationDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
