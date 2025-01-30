using AuctionManagement.WebAPI.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.WebAPI.Dtos {
    public class ItemDTOCreate {
        [Required(ErrorMessage = "The item name is required.")]
        [MaxLength(60, ErrorMessage = "The item name cannot exceed 60 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "The item price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The item status is required.")]
        public Status Status { get; set; }

        [Required(ErrorMessage = "The item category id is required.")]
        public int CategoryId { get; set; }
    }
}
