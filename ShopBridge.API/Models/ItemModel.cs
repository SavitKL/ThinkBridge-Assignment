using System;
using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Models
{
    public class ItemModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Price field must be greater than 0")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Quantity field must be greater than 0")]
        public decimal Quantity { get; set; }
    }
}
