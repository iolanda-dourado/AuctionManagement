using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Implementation;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {
    /// <summary>
    /// Classe que controla as requisições HTTP
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]

    public class ItemsController : Controller {

        private readonly IItemsService itemsService;

        public ItemsController(IItemsService itemsService) {
            this.itemsService = itemsService;
        }


        /// <summary>
        /// Method that register a new item to the database
        /// </summary>
        /// <param name="itemDTOCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<IEnumerable<ItemDTO>> Add(ItemDTOCreate itemDTOCreate) {
            try {
                var createdItem = itemsService.AddItem(itemDTOCreate);

                if (createdItem == null) {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(Add), new { id = createdItem.Id }, createdItem);
            }
            catch (InvalidOperationException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<ItemDTO>> GetItems() {
            try {
                var itemsDTO = itemsService.GetItems();
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get an item by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<ItemDTO>> GetById(int id) {
            try {
                List<ItemDTO> items = itemsService.GetItems();
                ItemDTO item = itemsService.GetItemById(id);
                return Ok(item);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method that update an item by recieving its id and its JSON body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Update(int id, Item item) {
            try {
                ItemDTO updatedItem = itemsService.UpdateItem(id, item);
                return Ok(updatedItem);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method that deletes an item with the id recieved as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            try {
                ItemDTO itemDTO = itemsService.DeleteItem(id);
                return NoContent();
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }



        /*
         * -------------------- EXTRA ENDPOINTS --------------------
         */

        /// <summary>
        /// Method to get all items from a certain category
        /// </summary>
        /// <returns></returns>
        [HttpGet("categoryid/{categId}")]
        public ActionResult<IEnumerable<ItemDTO>> GetItemsByCategory(int categId) {
            try {
                var itemsDTO = itemsService.GetItemsByCategory(categId);
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all items until a certain price
        /// </summary>
        /// <returns></returns>
        [HttpGet("price/{price}")]
        public ActionResult<IEnumerable<ItemDTO>> GetItmsUntilPrice(decimal price) {
            try {
                var itemsDTO = itemsService.GetItemsUntilPrice(price);
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all items sold
        /// </summary>
        /// <returns></returns>
        [HttpGet("solditems")]
        public ActionResult<IEnumerable<ItemDTO>> GetItemsSold() {
            try {
                var itemsDTO = itemsService.GetItemsSold();
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all items available
        /// </summary>
        /// <returns></returns>
        [HttpGet("availableitems")]
        public ActionResult<IEnumerable<ItemDTO>> GetItemsNotSold() {
            try {
                var itemsDTO = itemsService.GetItemsNotSold();
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all items sold by category
        /// </summary>
        /// <returns></returns>
        [HttpGet("solditemsbycategory/{categId}")]
        public ActionResult<IEnumerable<ItemDTO>> GetItemsSoldByCategory(int categId) {
            try {
                var itemsDTO = itemsService.GetItemsSoldByCategory(categId);
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to get all items available
        /// </summary>
        /// <returns></returns>
        [HttpGet("availableitemsbycategory/{categId}")]
        public ActionResult<IEnumerable<ItemDTO>> GetItemsNotSoldByCategory(int categId) {
            try {
                var itemsDTO = itemsService.GetItemsNotSoldByCategory(categId);
                return Ok(itemsDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
