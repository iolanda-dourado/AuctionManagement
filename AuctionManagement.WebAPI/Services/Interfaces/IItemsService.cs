using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuctionManagement.WebAPI.Services.Interfaces {
    public interface IItemsService {

        public List<ItemDTO> GetItems();

        public ItemDTO GetItemById(int id);

        public ItemDTO AddItem(ItemDTOCreate item);

        public ItemDTO UpdateItem(int id, Item item);

        public ItemDTO DeleteItem(int id);
    }
}
