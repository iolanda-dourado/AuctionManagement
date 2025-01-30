using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Validators {
    public class SalesValidator {
        private readonly AuctionContext context;

        public SalesValidator(AuctionContext context) => this.context = context;

        public Sale ValidateSale(int id) {
            Sale sale = context.Sales.Find(id)!;

            if (sale == null)
                throw new InvalidOperationException("The provided sale id couldn't be found.")!;

            return sale;
        }

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


        public void ValidateFilteredList(List<SaleDTO> salesDTO) {
            if (salesDTO == null || salesDTO.Count == 0) {
                throw new InvalidOperationException("No sales attended to the criterias.");
            }
        }


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
