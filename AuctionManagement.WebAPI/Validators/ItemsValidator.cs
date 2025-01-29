using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Validators {
    public class ItemsValidator {
        private readonly AuctionContext context;

        public ItemsValidator(AuctionContext context) => this.context = context;

        public Item ValidateItemExistence(int itemId) {
            var item = context.Items.Find(itemId);
            if (item == null) {
                throw new InvalidOperationException("The provided item id doesn't match any existing item.");
            }

            return item;
        }


        public void ValidateItemStatus(Item item) {
            if (item.Status == Status.Sold) {
                throw new InvalidOperationException("The item was already sold.");
            }
        }


        public List<ItemDTO> ValidateItemsList() {
            var items = context.Items.ToList();
            var itemsDTO = new List<ItemDTO>();

            if (items == null || !items.Any()) {
                throw new InvalidOperationException("The items list is empty.");
            }
            
            foreach (Item item in items) {
                itemsDTO.Add(ItemDTO.FromItemToDTO(item)!);
            }

            return itemsDTO;
        }


        public void ValidateFilteredList(List<Item> items) {
            if (items == null || items.Count == 0) {
                throw new InvalidOperationException("No items attended to the criterias.");
            }
        }
    }
}
