using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionManagement.WebAPI.Models {

    /// <summary>
    /// Represents a sale in the auction.
    /// </summary>
    public class Sale {

        /// <summary>
        /// The id of the sale.
        /// </summary>
        [Key]
        public int Id {  get; set; }


        /// <summary>
        /// The date of the sale.
        /// </summary>
        [Required(ErrorMessage = "The date of sale is required")]
        public DateOnly Date { get; set; }


        /// <summary>
        /// The sale price.
        /// </summary>
        [Required(ErrorMessage = "The sale price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }


        /// <summary>
        /// The item id of the sale.
        /// </summary>
        [Required(ErrorMessage = "The item id is required.")]
        public int ItemId { get; set; }
    }
}
