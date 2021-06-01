using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Api.Dtos
{
    public class InventoryItemCreateDto
    {

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Required]
        [MaxLength(250)]
        public string Description { get; set; }


        [Range(1, 9999999999, ErrorMessage = "Price should be between 1 and 9999999999")]
        public decimal Price { get; set; }

    }
}