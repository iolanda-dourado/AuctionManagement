using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Services.Interfaces {
    public interface ISalesService {


        public SaleDTO AddSale(SaleDTOCreate saleDTO);

        public List<SaleDTO> GetSales();

        public SaleDTO GetSaleById(int id);

        public SaleDTO UpdateSale(int id, Sale sale);

        public SaleDTO DeleteSale(int id);


        // EXTRA ENDPOINTS
        public decimal GetTotalSalesValue();

        public List<SaleDTO> GetSalesPerPeriod(DateOnly date1, DateOnly date2);

        public List<SaleDTO> GetSalesAboveValue(decimal value);
    }
}
