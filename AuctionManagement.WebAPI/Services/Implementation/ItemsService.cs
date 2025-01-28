using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class ItemsService : IItemsService {
        
        private readonly AuctionContext _context;

        public ItemsService(AuctionContext context) {
            _context = context;
        }

        public List<ItemDTO> GetItems() {
            var itemsDTO = new List<ItemDTO>();

            var items = _context.Items
                .Include(i => i.Category)
                .ToList();

            if (items != null && items.Any()) {
                foreach (Item item in items)
                    itemsDTO.Add(ItemDTO.FromItemToDTO(item));

                return itemsDTO;
            }

            return null;
        }


        public ItemDTO GetItemById(int id) {
            Item item = _context.Items.Find(id)!;

            if (item == null || item.Price < 1)
                return null!;

            return ItemDTO.FromItemToDTO(item)!;
        }

        public ItemDTO AddItem(ItemDTOCreate itemDTO) {
            if (itemDTO.Price < 1) {
                return null;
            }

            var category = _context.Categories.Find(itemDTO.CategoryId);

            if (category == null) {
                throw new ArgumentException("The provided category wasn't found.");
            }

            Item item = new Item {
                Name = itemDTO.Name,
                Price = itemDTO.Price,
                Status = itemDTO.Status,
                CategoryId = category.Id,
                Category = category
            };

            _context.Items.Add(item);
            _context.SaveChanges();

            return ItemDTO.FromItemToDTO(item);
        }

        public ItemDTO UpdateItem(int id, Item item) {
            var existingItem = _context.Items.Find(id);
            if (existingItem == null) return null!;

            _context.Entry(existingItem).CurrentValues.SetValues(item);
            _context.SaveChanges();

            return ItemDTO.FromItemToDTO(existingItem)!;
        }

        public ItemDTO DeleteItem(int id) {
            Item item = _context.Items.Find(id)!;

            if (item != null) {
                _context.Remove(item);
                _context.SaveChanges();
            }

            return ItemDTO.FromItemToDTO(item!)!;
        }
    }
}
