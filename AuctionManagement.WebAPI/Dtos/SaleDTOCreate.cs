using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionManagement.WebAPI.Dtos {
    public class SaleDTOCreate {

        [Required(ErrorMessage = "The sale price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The item id is required.")]
        public int ItemId { get; set; }
    }
}
