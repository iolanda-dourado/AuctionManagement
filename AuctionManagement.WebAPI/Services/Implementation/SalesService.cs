using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class SalesService : ISalesService {

        private readonly AuctionContext context;
        private readonly SalesValidator salesValidator;
        private readonly ItemsValidator itemsValidator;
        private readonly CategoriesValidator categoriesValidator;

        public SalesService(AuctionContext context, ItemsValidator itemsValidator, SalesValidator salesValidator, CategoriesValidator categoriesValidator) {
            this.context = context;
            this.itemsValidator = itemsValidator;
            this.salesValidator = salesValidator;
            this.categoriesValidator = categoriesValidator;
        }


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


        public decimal GetTotalSalesValueByCategory(int categId) {
            return context.Items.Where(i => i.CategoryId == categId).Sum(i => i.Price);
        }


        public int GetTotalSalesQuantity() {
            salesValidator.ValidateSalesList();
            int totalQuantity = context.Sales.Count();

            return totalQuantity;
        }

        public int GetTotalSalesQuantityByCategory(int categId) {
            return context.Items.Count(i => i.CategoryId == categId);
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