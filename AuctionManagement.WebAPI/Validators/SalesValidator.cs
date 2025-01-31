using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Validators {

    /// <summary>
    /// Validates sales based on various criteria.
    /// </summary>
    public class SalesValidator {

        /// <summary>
        /// The database context.
        /// </summary>
        private readonly AuctionContext context;

        /// <summary>
        /// Initializes a new instance of the SalesValidator class.
        /// </summary>
        /// <param name="context"></param>
        public SalesValidator(AuctionContext context) => this.context = context;


        /// <summary>
        /// Validates a sale by its ID and returns the sale if it exists.
        /// </summary>
        /// <param name="id">The ID of the sale to validate.</param>
        /// <returns>The sale if it exists.</returns>
        /// <exception 
        public Sale ValidateSale(int id) {
            Sale sale = context.Sales.Find(id)!;

            if (sale == null)
                throw new InvalidOperationException("The provided sale id couldn't be found.")!;

            return sale;
        }


        /// <summary>
        /// Retrieves and validates the list of sales from the database context.
        /// </summary>
        /// <returns>A list of SaleDTO objects representing the validated sales.</returns>
        /// <exception cref="InvalidOperationException">If the sales list is empty.</exception>
        public List<SaleDTO> ValidateSalesList() {
            var sales = context.Sales.ToList();
            var salesDTO = new List<SaleDTO>();

            if (sales == null || !sales.Any())
                throw new InvalidOperationException("The sales list is empty.");

            foreach (Sale sale in sales) {
                salesDTO.Add(SaleDTO.FromSaleToDTO(sale)!);
            }

            return salesDTO;
        }


        /// <summary>
        /// Validates a filtered list of sales.
        /// </summary>
        /// <param name="salesDTO">The list of sales to validate.</param>
        /// <exception cref="InvalidOperationException">If the list is empty.</exception>
        public void ValidateFilteredList(List<SaleDTO> salesDTO) {
            if (salesDTO == null || salesDTO.Count == 0) {
                throw new InvalidOperationException("No sales attended to the criterias.");
            }
        }


        /// <summary>
        /// Validates the price of a sale.
        /// </summary>
        /// <param name="saleDTOCreate">The sale to validate.</param>
        /// <exception cref="ArgumentException">If the sale price is less than the item price.</exception>
        public void ValidateSalePrice(SaleDTOCreate saleDTOCreate) {
            Item item = context.Items.Find(saleDTOCreate.ItemId)!;
            if (saleDTOCreate.Price < item.Price) {
                throw new ArgumentException("The sale price cannot be lesser than the item price.");
            }
        }


        //public void ValidateSaleDate(DateOnly saleDate) {
        //    var currentDate = DateOnly.FromDateTime(DateTime.Now);
        //    var maxDate = new DateOnly(2024, 1, 28);
        //    if (saleDate < maxDate || saleDate > currentDate) {
        //        throw new InvalidOperationException("The sale date must be between 28/01/2024 and today.");
        //    }
        //}
    }
}
