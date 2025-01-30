using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class ItemsService : IItemsService {

        private readonly AuctionContext context;
        private readonly ItemsValidator itemsValidator;
        private readonly CategoriesValidator categoriesValidator;

        public ItemsService(AuctionContext context, ItemsValidator itemsValidator, CategoriesValidator categoriesValidator) {
            this.context = context;
            this.itemsValidator = itemsValidator;
            this.categoriesValidator = categoriesValidator;
        }


        public ItemDTO AddItem(ItemDTOCreate itemDTO) {
            Category category = categoriesValidator.ValidateCategoryExistence(itemDTO.CategoryId);


            Item item = new Item {
                Name = itemDTO.Name,
                Price = itemDTO.Price,
                Status = Status.Available,
                CategoryId = category.Id,
                Category = category
            };

            context.Items.Add(item);
            context.SaveChanges();

            return ItemDTO.FromItemToDTO(item)!;
        }


        public List<ItemDTO> GetItems() {
            List<ItemDTO> itemsDTO = itemsValidator.ValidateItemsList();

            return itemsDTO;
        }


        public ItemDTO GetItemById(int id) {
            Item item = itemsValidator.ValidateItemExistence(id);

            return ItemDTO.FromItemToDTO(item)!;
        }


        public ItemDTO UpdateItem(int id, ItemDTOUpdate item) {
            Item existingItem = itemsValidator.ValidateItemExistence(id);
            Category category = categoriesValidator.ValidateCategoryExistence(item.CategoryId);
            itemsValidator.ValidateItemStatus(existingItem);

            context.Entry(existingItem).CurrentValues.SetValues(item);
            context.SaveChanges();

            return ItemDTO.FromItemToDTO(existingItem)!;
        }

        public ItemDTO DeleteItem(int id) {
            Item item = itemsValidator.ValidateItemExistence(id);

            context.Remove(item);
            context.SaveChanges();

            return ItemDTO.FromItemToDTO(item!)!;
        }




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */

        public List<ItemDTO> GetItemsByCategory(int categId) {
            Category category = categoriesValidator.ValidateCategoryExistence(categId);

            List<Item> filteredItems = context.Items.Where(i => i.CategoryId == category.Id).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        public List<ItemDTO> GetItemsUntilPrice(decimal price) {
            List<Item> filteredItems = context.Items.Where(i => i.Price <= price).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        public List<ItemDTO> GetItemsSold() {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Sold).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }

        public List<ItemDTO> GetItemsNotSold() {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Available).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }

        public List<ItemDTO> GetItemsSoldByCategory(int categId) {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Sold && i.CategoryId == categId).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }

        public List<ItemDTO> GetItemsNotSoldByCategory(int categId) {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Available && i.CategoryId == categId).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }
    }
}
