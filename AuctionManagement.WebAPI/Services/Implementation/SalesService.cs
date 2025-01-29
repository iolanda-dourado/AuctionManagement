﻿using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class SalesService : ISalesService {

        private readonly AuctionContext context;
        private readonly SalesValidator salesValidator;
        private readonly ItemsValidator itemsValidator;

        public SalesService(AuctionContext context, ItemsValidator itemsValidator, SalesValidator salesValidator) {
            this.context = context;
            this.itemsValidator = itemsValidator;
            this.salesValidator = salesValidator;
        }


        public SaleDTO AddSale(SaleDTOCreate saleDTO) {
            var item = itemsValidator.ValidateItemExistence(saleDTO.ItemId);
            salesValidator.ValidateSaleDate(saleDTO.Date);
            itemsValidator.ValidateItemStatus(item);

            Sale sale = new Sale {
                Date = saleDTO.Date,
                Price = saleDTO.Price,
                ItemId = saleDTO.ItemId,
            };

            context.Items.Find(saleDTO.ItemId)!.Status = Status.Sold;

            context.Sales.Add(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(sale);
        }


        public List<SaleDTO> GetSales() {
            List<SaleDTO> salesDTO = salesValidator.ValidateSalesList();

            return salesDTO;
        }


        public SaleDTO GetSaleById(int id) {
            Sale sale = salesValidator.ValidateSale(id);

            return SaleDTO.FromSaleToDTO(sale);
        }


        public SaleDTO UpdateSale(int id, Sale sale) {
            Sale existingSale = salesValidator.ValidateSale(id);
            salesValidator.ValidateSaleDate(sale.Date);

            context.Entry(existingSale).CurrentValues.SetValues(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(existingSale);
        }


        public SaleDTO DeleteSale(int id) {
            Sale sale = salesValidator.ValidateSale(id);

            context.Remove(sale);
            context.SaveChanges();

            return SaleDTO.FromSaleToDTO(sale);
        }




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */
        public decimal GetTotalSalesValue() {
            salesValidator.ValidateSalesList();
            decimal totalValue = context.Sales.Sum(s => s.Price);

            return totalValue;
        }


        public List<SaleDTO> GetSalesPerPeriod(DateOnly date1, DateOnly date2) {
            if (date2 < date1) {
                throw new ArgumentException("The second date must be greater than the first.");
            }

            salesValidator.ValidateSalesList();
            List<Sale> filteredList = context.Sales.Where(s => s.Date >= date1 && s.Date <= date2).ToList();
            List<SaleDTO> filteredListDTO = filteredList.ConvertAll(sale => SaleDTO.FromSaleToDTO(sale)!);
            salesValidator.ValidateFilteredList(filteredListDTO);

            return filteredListDTO;
        }


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