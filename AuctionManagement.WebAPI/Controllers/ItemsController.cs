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

        private readonly IItemsService _itemsService;

        public ItemsController(IItemsService itemsService) {
            _itemsService = itemsService;
        }



        /// <summary>
        /// Method to get all items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<ItemDTO>> GetItems() {
            var itemsDTO = _itemsService.GetItems();

            if (itemsDTO == null || !itemsDTO.Any()) {
                return NoContent();
            }

            return Ok(_itemsService.GetItems());
        }

        /// <summary>
        /// Method to get an item by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<ItemDTO> GetById(int id) {

            if (_itemsService == null)
                return NotFound();

            ItemDTO item = _itemsService.GetItemById(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }


        /// <summary>
        /// Method that register a new item to the database
        /// </summary>
        /// <param name="itemDTOCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<ItemDTO> Add(ItemDTOCreate itemDTOCreate) {
            try {
                var createdItem = _itemsService.AddItem(itemDTOCreate);

                if (createdItem == null) {
                    return BadRequest();
                }

                return CreatedAtAction(nameof(Add), new { id = createdItem.Id }, createdItem);
            }
            catch (ArgumentException ex) {
                return BadRequest(ex.Message);
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

            if (item.Id != id) {
                return BadRequest();
            }

            ItemDTO updatedItem = _itemsService.UpdateItem(id, item);

            return Ok(updatedItem);
        }


        /// <summary>
        /// Method that deletes an item with the id recieved as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            if (_itemsService == null)
                return NoContent();

            ItemDTO itemDTO = _itemsService.GetItemById(id);

            if (itemDTO == null)
                return NotFound();

            _itemsService.DeleteItem(id);
            return NoContent();
        }

    }
}
