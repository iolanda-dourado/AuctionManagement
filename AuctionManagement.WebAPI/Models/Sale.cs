using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionManagement.WebAPI.Models {
    public class Sale {

        [Key]
        public int Id {  get; set; }

        [Required(ErrorMessage = "The date of sale is required")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "The sale price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The item id is required.")]
        public int ItemId { get; set; }
    }
}
