using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Dtos {
    public class SaleDTO {

        public int Id { get; set; }

        [Required(ErrorMessage = "The date of sale is required")]
        public DateOnly Date { get; set; }

        [Required(ErrorMessage = "The sale price is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "The price must be greater than zero.")]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The item id is required.")]
        public int ItemId { get; set; }

        public SaleDTO(int id, DateOnly date, decimal price, int itemId) {
            Id = id;
            Date = date;
            Price = price;
            ItemId = itemId;
        }

        public static SaleDTO? FromSaleToDTO(Sale sale) {
            if (sale != null) {
                return new SaleDTO(sale.Id, sale.Date, sale.Price, sale.ItemId);
            }
            return null;
        }
    }
}
