using AuctionManagement.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Dtos {
    public class CategoryDTO {

        public int Id { get; set; }

        [Required(ErrorMessage = "The category description is required.")]
        [MaxLength(60, ErrorMessage = "The category description cannot exceed 60 characters.")]
        public string Description { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = new List<Item>();

        public CategoryDTO(int id, string description) {
            Id = id;
            Description = description;
            Items = new List<Item>();
        }

        public static CategoryDTO? FromCategoryToDTO(Category category) {
            if (category != null) {
                return new CategoryDTO(category.Id, category.Description);
            }
            return null;
        }
    }
}
