using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Models {
    public class Item {

        [Key]
        public int Id { get; set; }

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

        [JsonIgnore]
        public Category? Category { get; set; }


        public static Item FromDTOToItem(ItemDTO itemDTO) {
            return new Item { 
                Id = itemDTO.Id, 
                Name = itemDTO.Name, 
                Price = itemDTO.Price, 
                Status = itemDTO.Status, 
                CategoryId = itemDTO.CategoryId, 
                Category = itemDTO.Category 
            };
        }
    }
}
