using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.WebAPI.Dtos {

    /// <summary>
    /// Class SaleDTOCreate for creating a new sale
    /// </summary>
    public class SaleDTOCreate {

        /// <summary>
        /// The sale price
        /// </summary>
        [Required(ErrorMessage = "The sale price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }


        /// <summary>
        /// The item id
        /// </summary>
        [Required(ErrorMessage = "The item id is required.")]
        public int ItemId { get; set; }
    }
}
