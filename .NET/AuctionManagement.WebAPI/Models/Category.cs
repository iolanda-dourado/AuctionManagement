using AuctionManagement.WebAPI.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Models {
    
    /// <summary>
    /// Represents a category.
    /// </summary>
    public class Category {

        /// <summary>
        /// The category identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// The category description.
        /// </summary>
        [Required(ErrorMessage = "The category description is required.")]
        [MaxLength(60, ErrorMessage = "The category description cannot exceed 60 characters.")]
        public string Description { get; set; } = null!;


        /// <summary>
        /// The items associated with the category.
        /// </summary>
        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = new List<Item>();


        /// <summary>
        /// Default constructor.
        /// </summary>
        public Category() { }


        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public Category(int id, string description) {
            Id = id;
            Description = description;
        }


        /// <summary>
        /// Converts a CategoryDTO to a Category.
        /// </summary>
        /// <param name="categoryDTO"></param>
        /// <returns></returns>
        public static Category FromDTOToCategory(CategoryDTO categoryDTO) {
            return new Category(categoryDTO.Id, categoryDTO.Description);
        }


        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) {
            return obj is Category category &&
                   Description == category.Description;
        }
    }
}
