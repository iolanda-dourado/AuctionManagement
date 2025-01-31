using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Dtos {

    /// <summary>
    /// Data transfer object for an item, used for serialization and deserialization.
    /// </summary>
    public class ItemDTO {

        /// <summary>
        /// Unique identifier for the item.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Name of the item.
        /// </summary>
        [Required(ErrorMessage = "The item name is required.")]
        [MaxLength(60, ErrorMessage = "The item name cannot exceed 60 characters.")]
        public string Name { get; set; } = null!;


        /// <summary>
        /// Item price property
        /// </summary>
        [Required(ErrorMessage = "The item price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }


        /// <summary>
        /// Status of the item
        /// </summary>
        [Required(ErrorMessage = "The item status is required.")]
        public Status Status { get; set; }


        /// <summary>
        /// Item category id
        /// </summary>
        [Required(ErrorMessage = "The item category id is required.")]
        public int CategoryId { get; set; }


        /// <summary>
        /// Item category
        /// </summary>
        [JsonIgnore]
        public Category? Category { get; set; } = null!;


        /// <summary>
        /// Item constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="price"></param>
        /// <param name="status"></param>
        /// <param name="idCategory"></param>
        /// <param name="category"></param>
        public ItemDTO(int id, string name, decimal price, Status status, int idCategory, Category category) {
            Id = id;
            Name = name;
            Price = price;
            Status = status;
            CategoryId = idCategory;
            Category = category;
        }


        /// <summary>
        /// Method to convert item to itemDTO
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ItemDTO? FromItemToDTO(Item item) {
            if (item != null) {
                return new ItemDTO(item.Id, item.Name, item.Price, item.Status, item.CategoryId, item.Category);
            }
            return null;
        }
    }
}
