using AuctionManagement.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Dtos {
    
    /// <summary>
    /// DTO for creating a new category
    /// </summary>
    public class CategoryDTOCreate {

        /// <summary>
        /// Category description
        /// </summary>
        [Required(ErrorMessage = "The category description is required.")]
        [MaxLength(60, ErrorMessage = "The category description cannot exceed 60 characters.")]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Items
        /// </summary>
        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
