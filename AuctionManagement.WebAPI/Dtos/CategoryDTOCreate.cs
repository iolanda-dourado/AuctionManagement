using AuctionManagement.WebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionManagement.WebAPI.Dtos {
    public class CategoryDTOCreate {

        [Required(ErrorMessage = "The category description is required.")]
        [MaxLength(60, ErrorMessage = "The category description cannot exceed 60 characters.")]
        public string Description { get; set; } = null!;

        [JsonIgnore]
        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
