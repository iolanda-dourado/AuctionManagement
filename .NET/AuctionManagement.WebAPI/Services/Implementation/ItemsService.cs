using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {

    /// <summary>
    /// Provides methods for managing items in the auction system.
    /// </summary>
    public class ItemsService : IItemsService {

        /// <summary>
        /// The database context for the auction system.
        /// </summary>
        private readonly AuctionContext context;

        /// <summary>
        /// The validator for items in the auction system.
        /// </summary>
        private readonly ItemsValidator itemsValidator;

        /// <summary>
        /// The validator for categories in the auction system.
        /// </summary>
        private readonly CategoriesValidator categoriesValidator;

        /// <summary>
        /// Initializes a new instance of the ItemsService class.
        /// </summary>
        /// <param name="context">The database context for the auction system.</param>
        /// <param name="itemsValidator">The validator for items in the auction system.</param>
        /// <param name="categoriesValidator">The validator for categories in the auction system.</param>
        public ItemsService(AuctionContext context, ItemsValidator itemsValidator, CategoriesValidator categoriesValidator) {
            this.context = context;
            this.itemsValidator = itemsValidator;
            this.categoriesValidator = categoriesValidator;
        }


        /// <summary>
        /// Adds a new item to the auction system.
        /// </summary>
        /// <param name="itemDTO">The item to add.</param>
        /// <returns>The added item as a DTO.</returns>
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


        /// <summary>
        /// Retrieves a list of all items in the auction system.
        /// </summary>
        /// <returns>A list of items as DTOs.</returns>
        public List<ItemDTO> GetItems() {
            itemsValidator.ValidateItemsList();

            List<Item> filteredItems = context.Items
                .Include(i => i.Category)
                .ToList();

            return filteredItems.Select(ItemDTO.FromItemToDTO).ToList();
        }



        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item to retrieve.</param>
        /// <returns>The item as a DTO, or null if not found.</returns>
        public ItemDTO GetItemById(int id) {
            Item item = itemsValidator.ValidateItemExistence(id);

            item = context.Items
                .Where(i => i.Id == id)
                .Include(i => i.Category)
                .FirstOrDefault()!;

            return ItemDTO.FromItemToDTO(item)!;
        }



        /// <summary>
        /// Updates an existing item in the auction system.
        /// </summary>
        /// <param name="id">The ID of the item to update.</param>
        /// <param name="item">The updated item.</param>
        /// <returns>The updated item as a DTO.</returns>
        public ItemDTO UpdateItem(int id, ItemDTOUpdate item) {
            Item existingItem = itemsValidator.ValidateItemExistence(id);
            Category category = categoriesValidator.ValidateCategoryExistence(item.CategoryId);
            itemsValidator.ValidateItemStatus(existingItem);

            context.Entry(existingItem).CurrentValues.SetValues(item);
            context.SaveChanges();

            return ItemDTO.FromItemToDTO(existingItem)!;
        }


        /// <summary>
        /// Deletes an item from the auction system.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>The deleted item as a DTO.</returns>
        public ItemDTO DeleteItem(int id) {
            Item item = itemsValidator.ValidateItemExistence(id);

            context.Remove(item);
            context.SaveChanges();

            return ItemDTO.FromItemToDTO(item!)!;
        }




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */


        /// <summary>
        /// Retrieves a list of items in a specific category.
        /// </summary>
        /// <param name="categId">The ID of the category to retrieve items from.</param>
        /// <returns>A list of items in the category as DTOs.</returns>
        public List<ItemDTO> GetItemsByCategory(int categId) {
            Category category = categoriesValidator.ValidateCategoryExistence(categId);

            List<Item> filteredItems = context.Items.Where(i => i.CategoryId == category.Id).ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }



        /// <summary>
        /// Retrieves a list of items in the auction system that have a price less than or equal to 
        /// the specified price.
        /// </summary>
        /// <param name="price">The maximum price of the items to retrieve.</param>
        /// <returns>A list of items with prices less than or equal to the specified price as DTOs.</returns>
        public List<ItemDTO> GetItemsUntilPrice(decimal price) {
            List<Item> filteredItems = context.Items
                .Where(i => i.Price <= price)
                .Include(i => i.Category)
                .ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        /// <summary>
        /// Retrieves a list of items in the auction system that have not been sold.
        /// </summary>
        /// <returns>A list of unsold items as DTOs.</returns>
        public List<ItemDTO> GetItemsAvailable() {
            List<Item> filteredItems = context.Items
                .Include(i => i.Category) // Carrega a entidade relacionada Category
                .Where(i => i.Status == Enums.Status.Available)
                .ToList();

            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        public List<ItemDTO> GetItemsAtAuction() {
            List<Item> filteredItems = context.Items
                .Include(i => i.Category)
                .Where(i => i.Status == Enums.Status.AtAuction)
                .ToList();

            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        /// <summary>
        /// Retrieves a list of items in the auction system that have been sold.
        /// </summary>
        /// <returns>A list of sold items as DTOs.</returns>
        public List<ItemDTO> GetItemsSold() {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Sold)
                .Include(i => i.Category)
                .ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        /// <summary>
        /// Retrieves a list of items in the auction system that have not been sold.
        /// </summary>
        /// <returns>A list of unsold items as DTOs.</returns>
        public List<ItemDTO> GetItemsSoldByCategory(int categId) {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Sold && i.CategoryId == categId)
                .Include(i => i.Category)
                .ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        /// <summary>
        /// Retrieves a list of items in a specific category that have not been sold.
        /// </summary>
        /// <param name="categId">The ID of the category to retrieve unsold items from.</param>
        /// <returns>A list of unsold items in the category as DTOs.</returns>
        public List<ItemDTO> GetItemsNotSoldByCategory(int categId) {
            List<Item> filteredItems = context.Items.
                Where(i => i.Status == Enums.Status.Available && i.CategoryId == categId)
                .Include(i => i.Category)
                .ToList();
            itemsValidator.ValidateFilteredList(filteredItems);

            List<ItemDTO> filteredItemsDTO = filteredItems.ConvertAll(item => ItemDTO.FromItemToDTO(item)!);

            return filteredItemsDTO;
        }


        public Status UpdateItemStatus(int itemId, int status) {
            Item item = itemsValidator.ValidateItemExistence(itemId);

            if (!Enum.IsDefined(typeof(Status), status)) {
                throw new ArgumentException("Invalid status value.");
            }

            Status statusEnum = (Status)status;

            if (item.Status != Status.Sold) {
                item.Status = statusEnum;
                context.SaveChanges();
                return item.Status;
            } else {
                throw new InvalidOperationException("Item is already sold.");
            }
        }
    }
}
