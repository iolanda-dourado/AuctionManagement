using AuctionManagement.WebAPI.Dtos;

namespace AuctionManagement.WebAPI.Services.Interfaces {

    /// <summary>
    /// Defines the interface for sales services.
    /// </summary>
    public interface ISalesService {

        /// <summary>
        /// Adds a new sale.
        /// </summary>
        /// <param name="saleDTO">The sale data transfer object to add.</param>
        /// <returns>The added sale data transfer object.</returns>
        public SaleDTO AddSale(SaleDTOCreate saleDTO);


        /// <summary>
        /// Retrieves a list of all sales.
        /// </summary>
        /// <returns>A list of sale data transfer objects.</returns>
        public List<SaleDTO> GetSales();


        /// <summary>
        /// Retrieves a sale by its ID.
        /// </summary>
        /// <param name="id">The ID of the sale to retrieve.</param>
        /// <returns>The sale data transfer object with the specified ID.</returns>
        public SaleDTO GetSaleById(int id);

        //public SaleDTO UpdateSale(int id, Sale sale);

        //public SaleDTO DeleteSale(int id);


        // EXTRA ENDPOINTS

        /// <summary>
        /// Retrieves the total value of all sales.
        /// </summary>
        /// <returns>The total value of all sales.</returns>
        public decimal GetTotalSalesValue();


        /// <summary>
        /// Retrieves the total value of sales by category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// /// <returns>The total value of sales for the specified category.</returns>
        public decimal GetTotalSalesValueByCategory(int categId);


        /// <summary>
        /// Retrieves the total quantity of all sales.
        /// </summary>
        /// <returns>The total quantity of all sales.</returns>
        public int GetTotalSalesQuantity();


        /// <summary>
        /// Retrieves the total quantity of sales by category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// <returns>The total quantity of sales for the specified category.</returns>
        public int GetTotalSalesQuantityByCategory(int categId);


        /// <summary>
        /// Retrieves a list of sales that occurred within a specified period.
        /// </summary>
        /// <param name="date1">The start date of the period.</param>
        /// <param name="date2">The end date of the period.</param>
        /// <returns>A list of sale data transfer objects that occurred within the specified period.</returns>
        public List<SaleDTO> GetSalesPerPeriod(DateOnly date1, DateOnly date2);


        /// <summary>
        /// Retrieves a list of sales with a value above a specified threshold.
        /// </summary>
        /// <param name="value">The minimum value of sales to retrieve.</param>
        /// <returns>A list of sale data transfer objects with a value above the specified threshold.</returns>
        public List<SaleDTO> GetSalesAboveValue(decimal value);
    }
}
