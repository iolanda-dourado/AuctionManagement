using AuctionManagement.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Dtos {
    
    /// <summary>
    /// Category data transfer object
    /// </summary>
    public class CategoryDTO {

        /// <summary>
        /// Category id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Category description
        /// </summary>
        [Required(ErrorMessage = "The category description is required.")]
        [MaxLength(60, ErrorMessage = "The category description cannot exceed 60 characters.")]
        public string Description { get; set; } = null!;


        /// <summary>
        /// Items of the category
        /// </summary>
        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = new List<Item>();


        /// <summary>
        /// Category constructor method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public CategoryDTO(int id, string description) {
            Id = id;
            Description = description;
            Items = new List<Item>();
        }


        /// <summary>
        /// Method to convert category to categoryDTO
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static CategoryDTO? FromCategoryToDTO(Category category) {
            if (category != null) {
                return new CategoryDTO(category.Id, category.Description);
            }
            return null;
        }
    }
}
