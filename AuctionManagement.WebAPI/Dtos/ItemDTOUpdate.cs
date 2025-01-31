using AuctionManagement.WebAPI.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.WebAPI.Dtos {

    /// <summary>
    /// Represents the data transfer object for updating an item.
    /// </summary>
    public class ItemDTOUpdate {

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The name of the item.
        /// </summary>
        [Required(ErrorMessage = "The item name is required.")]
        [MaxLength(60, ErrorMessage = "The item name cannot exceed 60 characters.")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// The price of the item.
        /// </summary>
        [Required(ErrorMessage = "The item price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }


        /// <summary>
        /// The status of the item.
        /// </summary>
        [Required(ErrorMessage = "The item category id is required.")]
        public int CategoryId { get; set; }
    }
}
