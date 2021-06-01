using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Api.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Range(1, 9999999999)]
        public decimal Price { get; set; }

    }
}