using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Dtos {
    public class ItemDTO {

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
        public Category? Category { get; set; } = null!;


        public ItemDTO(int id, string name, decimal price, Status status, int idCategory, Category category) {
            Id = id;
            Name = name;
            Price = price;
            Status = status;
            CategoryId = idCategory;
            Category = category;
        }

        public static ItemDTO? FromItemToDTO(Item item) {
            if (item != null) {
                return new ItemDTO(item.Id, item.Name, item.Price, item.Status, item.CategoryId, item.Category);
            }
            return null;
        }
    }
}
