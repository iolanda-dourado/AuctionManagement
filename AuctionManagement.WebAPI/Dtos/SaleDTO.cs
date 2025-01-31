using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Dtos {

    /// <summary>
    /// Sale data transfer object
    /// </summary>
    public class SaleDTO {

        /// <summary>
        /// The sale id
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The date of sale
        /// </summary>
        [Required(ErrorMessage = "The date of sale is required")]
        public DateOnly Date { get; set; }


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


        /// <summary>
        /// Sale constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="price"></param>
        /// <param name="itemId"></param>
        public SaleDTO(int id, DateOnly date, decimal price, int itemId) {
            Id = id;
            Date = date;
            Price = price;
            ItemId = itemId;
        }


        /// <summary>
        /// Method to convert sale to saleDTO
        /// </summary>
        /// <param name="sale"></param>
        /// <returns></returns>
        public static SaleDTO? FromSaleToDTO(Sale sale) {
            if (sale != null) {
                return new SaleDTO(sale.Id, sale.Date, sale.Price, sale.ItemId);
            }
            return null;
        }
    }
}
