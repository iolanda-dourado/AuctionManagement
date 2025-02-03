using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {

    /// <summary>
    /// Provides methods for managing sales in the auction system.
    /// </summary>
    public class SalesService : ISalesService {

        /// <summary>
        /// The database context for the auction system.
        /// </summary>
        private readonly AuctionContext context;

        /// <summary>
        /// The validator for sales in the auction system.
        /// </summary>
        private readonly SalesValidator salesValidator;

        /// <summary>
        /// The validator for items in the auction system.
        /// </summary>
        private readonly ItemsValidator itemsValidator;

        /// <summary>
        /// The validator for categories in the auction system.
        /// </summary>
        private readonly CategoriesValidator categoriesValidator;


        /// <summary>
        /// Initializes a new instance of the SalesService class.
        /// </summary>
        /// <param name="context">The database context for the auction system.</param>
        /// <param name="itemsValidator">The validator for items in the auction system.</param>
        /// <param name="salesValidator">The validator for sales in the auction system.</param>
        /// <param name="categoriesValidator">The validator for categories in the auction system.</param>
        public SalesService(AuctionContext context, ItemsValidator itemsValidator, SalesValidator salesValidator, CategoriesValidator categoriesValidator) {
            this.context = context;
            this.itemsValidator = itemsValidator;
            this.salesValidator = salesValidator;
            this.categoriesValidator = categoriesValidator;
        }


        /// <summary>
        /// Adds a new sale to the auction system.
        /// </summary>
        /// <param name="saleDTOCreate">The sale to add.</param>
        /// <returns>The added sale as a DTO.</returns>
        public SaleDTO AddSale(SaleDTOCreate saleDTOCreate) {
            var item = itemsValidator.ValidateItemExistence(saleDTOCreate.ItemId);
            itemsValidator.ValidateItemStatus(item);
            salesValidator.ValidateSalePrice(saleDTOCreate);

            Sale sale = new Sale {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Price = saleDTOCreate.Price,
                ItemId = saleDTOCreate.ItemId,
            };

            context.Items.Find(saleDTOCreate.ItemId)!.Status = Status.Sold;

            context.Sales.Add(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(sale);
        }


        /// <summary>
        /// Retrieves a list of all sales in the auction system.
        /// </summary>
        /// <returns>A list of sales as DTOs.</returns>
        public List<SaleDTO> GetSales() {
            List<SaleDTO> salesDTO = salesValidator.ValidateSalesList();

            return salesDTO;
        }


        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <returns>The sale as a DTO.</returns>
        public SaleDTO GetSaleById(int id) {
            Sale sale = salesValidator.ValidateSale(id);

            return SaleDTO.FromSaleToDTO(sale);
        }


        //public SaleDTO UpdateSale(int id, Sale sale) {
        //    Sale existingSale = salesValidator.ValidateSale(id);

        //    context.Entry(existingSale).CurrentValues.SetValues(sale);
        //    context.SaveChanges();

        //    return SaleDTO.FromSaleToDTO(existingSale);
        //}


        //public SaleDTO DeleteSale(int id) {
        //    Sale sale = salesValidator.ValidateSale(id);

        //    context.Remove(sale);
        //    context.SaveChanges();

        //    return SaleDTO.FromSaleToDTO(sale);
        //}




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */


        public decimal GetTotalSalesValue() {
            salesValidator.ValidateSalesList();
            decimal totalValue = context.Sales.Sum(s => s.Price);

            return totalValue;
        }


        /// <summary>
        /// Retrieves the total value of all sales in the auction system.
        /// </summary>
        /// <returns>The total value of all sales as a decimal.</returns>
        public decimal GetTotalSalesValueByCategory(int categId) {
            salesValidator.ValidateSalesList();
            categoriesValidator.ValidateCategoryExistence(categId);
            return context.Items.Where(i => i.CategoryId == categId).Sum(i => i.Price);
        }


        /// <summary>
        /// Retrieves the total quantity of all sales in the auction system.
        /// </summary>
        /// <returns>The total quantity of all sales as an integer.</returns>
        public int GetTotalSalesQuantity() {
            salesValidator.ValidateSalesList();
            int totalQuantity = context.Sales.Count();

            return totalQuantity;
        }


        /// <summary>
        /// Retrieves the total quantity of sales in a specific category.
        /// </summary>
        /// <param name="categId">The ID of the category to retrieve sales from.</param>
        /// <returns>The total quantity of sales in the category as an integer.</returns>
        public int GetTotalSalesQuantityByCategory(int categId) {
            salesValidator.ValidateSalesList();
            categoriesValidator.ValidateCategoryExistence(categId);
            return context.Items.Count(i => i.CategoryId == categId);
        }


        /// <summary>
        /// Retrieves a list of sales that occurred within a specified date range.
        /// </summary>
        /// <param name="date1">The start date of the range (inclusive).</param>
        /// <param name="date2">The end date of the range (inclusive).</param>
        /// <returns>A list of sales as DTOs that occurred between date1 and date2.</returns>
        public List<SaleDTO> GetSalesPerPeriod(DateOnly date1, DateOnly date2) {
            if (date2 < date1) {
                throw new ArgumentException("The second date must be greater than the first.");
            }

            salesValidator.ValidateSalesList();
            List<Sale> filteredList = context.Sales.Where(s => s.Date >= date1 && s.Date <= date2).ToList();
            List<SaleDTO> filteredListDTO = filteredList.ConvertAll(sale => SaleDTO.FromSaleToDTO(sale)!)
                .OrderBy(s => s.Date)
                .ToList();
            salesValidator.ValidateFilteredList(filteredListDTO);

            return filteredListDTO;
        }


        /// <summary>
        /// Retrieves a list of sales with a price greater than or equal to the specified value.
        /// </summary>
        /// <param name="value">The minimum price of the sales to retrieve.</param>
        /// <returns>A list of sales as DTOs with prices greater than or equal to the specified value.</returns>
        public List<SaleDTO> GetSalesAboveValue(decimal value) {
            if (value <= 0) {
                throw new ArgumentException("The value must be greater than 0.");
            }

            salesValidator.ValidateSalesList();
            List<Sale> filteredList = context.Sales.Where(s => s.Price >= value).ToList();
            List<SaleDTO> filteredListDTO = filteredList.ConvertAll(sale => SaleDTO.FromSaleToDTO(sale)!);
            salesValidator.ValidateFilteredList(filteredListDTO);

            return filteredListDTO;
        }

    }
}