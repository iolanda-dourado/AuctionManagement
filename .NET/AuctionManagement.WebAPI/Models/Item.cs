using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Models {

    /// <summary>
    /// Represents an item in the auction.
    /// </summary>
    public class Item {

        /// <summary>
        /// The item id.
        /// </summary>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// The item name.
        /// </summary>
        [Required(ErrorMessage = "The item name is required.")]
        [MaxLength(60, ErrorMessage = "The item name cannot exceed 60 characters.")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// The item price.
        /// </summary>
        [Required(ErrorMessage = "The item price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }


        /// <summary>
        /// The item status.
        /// </summary>
        [Required(ErrorMessage = "The item status is required.")]
        public Status Status { get; set; }


        /// <summary>
        /// The item category id.
        /// </summary>
        [Required(ErrorMessage = "The item category id is required.")]
        public int CategoryId { get; set; }


        /// <summary>
        /// The item category.
        /// </summary>
        [JsonIgnore]
        public Category? Category { get; set; }


        /// <summary>
        /// Converts an ItemDTO to an Item.
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns>The converted Item.</returns>
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
