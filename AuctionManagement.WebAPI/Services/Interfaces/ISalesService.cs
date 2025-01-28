using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Services.Interfaces {
    public interface ISalesService {


        public SaleDTO AddSale(SaleDTOCreate saleDTO);
        public List<SaleDTO> GetSales();
        
        public SaleDTO GetSaleById(int id);

        public SaleDTO UpdateSale(int id, Sale sale);

        public SaleDTO DeleteSale(int id);
    }
}
