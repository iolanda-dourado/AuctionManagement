using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuctionManagement.WebAPI.Services.Interfaces {

    /// <summary>
    /// Defines the interface for item services.
    /// </summary>
    public interface IItemsService {

        /// <summary>
        /// Adds a new item to the system.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>The added item.</returns>
        public ItemDTO AddItem(ItemDTOCreate item);

        /// <summary>
        /// Retrieves a list of all items.
        /// </summary>
        /// <returns>A list of items.</returns>
        public List<ItemDTO> GetItems();

        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item to retrieve.</param>
        /// <returns>The item with the specified ID, or null if not found.</returns>
        public ItemDTO GetItemById(int id);


        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="id">The ID of the item to update.</param>
        /// <param name="item">The updated item.</param>
        /// <returns>The updated item.</returns>
        public ItemDTO UpdateItem(int id, ItemDTOUpdate item);


        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>The deleted item.</returns>
        public ItemDTO DeleteItem(int id);


        // Extra endpoints

        /// <summary>
        /// Retrieves a list of items by category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// <returns>A list of items in the specified category.</returns>
        public List<ItemDTO> GetItemsByCategory(int categId);


        /// <summary>
        /// Retrieves a list of items with a price less than or equal to the specified price.
        /// </summary>
        /// <param name="price">The maximum price.</param>
        /// <returns>A list of items with a price less than or equal to the specified price.</returns>
        public List<ItemDTO> GetItemsUntilPrice(decimal price);


        /// <summary>
        /// Retrieves a list of unsold items.
        /// </summary>
        /// <returns>A list of unsold items.</returns>
        public List<ItemDTO> GetItemsAvailable();


        /// <summary>
        /// Retrieves a list of items that are currently up for auction.
        /// </summary>
        /// <returns></returns>
        public List<ItemDTO> GetItemsAtAuction();

        /// <summary>
        /// Retrieves a list of sold items.
        /// </summary>
        /// <returns>A list of sold items.</returns>
        public List<ItemDTO> GetItemsSold();

        /// <summary>
        /// Retrieves a list of sold items by category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// <returns>A list of sold items in the specified category.</returns>
        public List<ItemDTO> GetItemsSoldByCategory(int categId);


        /// <summary>
        /// Retrieves a list of unsold items by category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// <returns>A list of unsold items in the specified category.</returns>
        public List<ItemDTO> GetItemsNotSoldByCategory(int categId);


        /// <summary>
        /// Updates the status of an item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public Status UpdateItemStatus(int itemId, int status);
    }
}
