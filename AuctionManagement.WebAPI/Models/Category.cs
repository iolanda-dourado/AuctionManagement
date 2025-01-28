using AuctionManagement.WebAPI.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Models {
    public class Category {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The category description is required.")]
        [MaxLength(60, ErrorMessage = "The category description cannot exceed 60 characters.")]
        public string Description { get; set; } = null!;


        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = new List<Item>();

        public Category() { }

        public Category(int id, string description) {
            Id = id;
            Description = description;
        }


        public static Category FromDTOToCategory(CategoryDTO categoryDTO) {
            return new Category(categoryDTO.Id, categoryDTO.Description);
        }
    }
}
