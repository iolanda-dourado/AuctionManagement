using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Validators {

    /// <summary>
    /// Validates items based on various criteria.
    /// </summary>
    public class ItemsValidator {
        private readonly AuctionContext context;


        /// <summary>
        /// Initializes a new instance of the ItemsValidator class.
        /// </summary>
        /// <param name="context"></param>
        public ItemsValidator(AuctionContext context) => this.context = context;


        /// <summary>
        /// Initializes a new instance of the ItemsValidator class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public Item ValidateItemExistence(int itemId) {
            var item = context.Items.Find(itemId);
            if (item == null) {
                throw new InvalidOperationException("The provided item id doesn't match any existing item.");
            }

            return item;
        }


        /// <summary>
        /// Validates the status of an item to ensure it is not already sold.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        public void ValidateItemStatus(Item item) {
            if (item.Status == Status.Sold) {
                throw new InvalidOperationException("It was impossible to conclude this action because the item was already sold.");
            }
        }


        /// <summary>
        /// Retrieves and validates the list of items from the database context.
        /// </summary>
        /// <returns>A list of ItemDTO objects representing the validated items.</returns>
        public List<ItemDTO> ValidateItemsList() {
            var items = context.Items.ToList();
            var itemsDTO = new List<ItemDTO>();

            if (items == null || items.Count == 0) {
                throw new InvalidOperationException("The items list is empty.");
            }
            
            foreach (Item item in items) {
                itemsDTO.Add(ItemDTO.FromItemToDTO(item)!);
            }

            return itemsDTO;
        }


        /// <summary>
        /// Validates a filtered list of items to ensure it is not empty.
        /// </summary>
        /// <param name="items">The list of items to validate.</param>
        /// <exception cref="InvalidOperationException">If the list is empty.</exception>
        public void ValidateFilteredList(List<Item> items) {
            if (items == null || items.Count == 0) {
                throw new InvalidOperationException("No items attended to the criterias.");
            }
        }
    }
}
