using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Exceptions;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class SalesService : ISalesService {

        private readonly AuctionContext context;

        public SalesService(AuctionContext context) {
            this.context = context;
        }


        public SaleDTO AddSale(SaleDTOCreate saleDTO) {
            var item = context.Items.Find(saleDTO.ItemId);

            ValidateItemExistence(saleDTO.ItemId);
            ValidateSaleDate(saleDTO.Date);
            ValidateItemStatus(item);

            Sale sale = new Sale {
                Date = saleDTO.Date,
                Price = saleDTO.Price,
                ItemId = saleDTO.ItemId,
            };

            context.Sales.Add(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(sale);
        }


        public List<SaleDTO> GetSales() {
            List<SaleDTO> salesDTO = ValidateSalesList();

            return salesDTO;
        }


        public SaleDTO GetSaleById(int id) {
            Sale sale = ValidateSale(id);

            return SaleDTO.FromSaleToDTO(sale);
        }


        public SaleDTO UpdateSale(int id, Sale sale) {
            var existingSale = ValidateSale(id);
            ValidateSaleDate(sale.Date);

            context.Entry(existingSale).CurrentValues.SetValues(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(existingSale);
        }


        public SaleDTO DeleteSale(int id) {
            Sale sale = ValidateSale(id);

            context.Remove(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(sale);
        }


        private void ValidateItemExistence(int itemId) {
            var item = context.Items.Find(itemId);
            if (item == null) {
                throw new InvalidOperationException("The provided item id doesn't match any existing item.");
            }
        }

        private void ValidateSaleDate(DateOnly saleDate) {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var maxDate = new DateOnly(2024, 1, 28);
            if (saleDate < maxDate || saleDate > currentDate) {
                throw new InvalidOperationException("The sale date must be between 28/01/2024 and today.");
            }
        }


        private void ValidateItemStatus(Item item) {
            if (item.Status == Status.Sold) {
                throw new InvalidOperationException("The item was already sold.");
            }
        }

        private List<SaleDTO> ValidateSalesList() {
            var sales = context.Sales.ToList();
            var salesDTO = new List<SaleDTO>();

            if (sales == null || !sales.Any())
                throw new InvalidOperationException("The sales list is empty.");

            foreach (Sale sale in sales) {
                salesDTO.Add(SaleDTO.FromSaleToDTO(sale)!);
            }

            return salesDTO;
        }

        private Sale ValidateSale(int id) {
            Sale sale = context.Sales.Find(id)!;

            if (sale == null)
                throw new InvalidOperationException("The provided sale id couldn't be found.")!;

            if (id != sale.Id)
                throw new ArgumentException("The provided ids don't match.");

            return sale;
        }

    }

}