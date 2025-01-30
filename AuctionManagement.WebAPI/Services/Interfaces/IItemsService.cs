using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuctionManagement.WebAPI.Services.Interfaces {
    public interface IItemsService {

        public ItemDTO AddItem(ItemDTOCreate item);

        public List<ItemDTO> GetItems();

        public ItemDTO GetItemById(int id);

        public ItemDTO UpdateItem(int id, ItemDTOUpdate item);

        public ItemDTO DeleteItem(int id);


        // Extra endpoints
        public List<ItemDTO> GetItemsByCategory(int categId);

        public List<ItemDTO> GetItemsUntilPrice(decimal price);

        public List<ItemDTO> GetItemsSold();

        public List<ItemDTO> GetItemsNotSold();

        public List<ItemDTO> GetItemsSoldByCategory(int categId);

        public List<ItemDTO> GetItemsNotSoldByCategory(int categId);
    }
}
