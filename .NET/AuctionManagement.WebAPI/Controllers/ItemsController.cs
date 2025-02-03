using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {

    /// <summary>
    /// Controller for managing items in the auction management system.
    /// Provides endpoints for adding, retrieving, updating, and deleting items.
    /// </summary>
    /// <remarks>
    /// This controller requires authorization with the Admin role for most endpoints.
    /// </remarks>
    [Route("api/[controller]/")]
    [ApiController]
    public class ItemsController : Controller {

        private readonly IItemsService itemsService;

        public ItemsController(IItemsService itemsService) {
            this.itemsService = itemsService;
        }


        /// <summary>
        /// Method to add a new item to the database. Requires authorization with Admin role.
        /// </summary>
        /// <param name="itemDTOCreate">The item to be added</param>
        /// <returns>The added item. Returns BadRequest if the item cannot be added.</returns>
        [Authorize(Roles = "Admin")]
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
        /// Method to retrieve all items from the database.
        /// </summary>
        /// <returns>A list of ItemDTO objects representing the items in the database.</returns>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If an error occurs while retrieving the items, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [Authorize(Roles = "Admin")]
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
        /// Retrieves an item by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to retrieve.</param>
        /// <returns>An ActionResult containing the ItemDTO instance if found, otherwise a NotFound result with a descriptive message.</returns>
        /// <response code="200">The item was successfully retrieved.</response>
        /// <response code="404">The item was not found.</response>

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
        /// Updates an existing item in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the item to update.</param>
        /// <param name="item">The updated item data.</param>
        /// <returns>An ActionResult containing the updated ItemDTO instance if successful, otherwise a NotFound result with a descriptive message.</returns>
        /// <response code="200">The item was successfully updated.</response>
        /// <response code="404">The item was not found.</response>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If an error occurs while updating the item, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, ItemDTOUpdate item) {
            try {
                ItemDTO updatedItem = itemsService.UpdateItem(id, item);
                return Ok(updatedItem);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Deletes an item from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the item to delete.</param>
        /// <returns>An ActionResult containing a NoContent result if successful, otherwise a NotFound result with a descriptive message.</returns>
        /// <response code="200">The item was successfully deleted.</response>
        /// <response code="404">The item was not found.</response>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If an error occurs while deleting the item, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [Authorize(Roles = "Admin")]
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
        /// Retrieves a list of items belonging to a specific category.
        /// </summary>
        /// <param name="categId">The ID of the category to retrieve items from.</param>
        /// <returns>A list of ItemDTO objects representing the items in the specified category.</returns>
        /// <remarks>If no items are found in the specified category, a NotFound response is returned.</remarks>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while retrieving items from the service.</exception>

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
        /// Retrieves a list of items with prices less than or equal to the specified price.
        /// </summary>
        /// <param name="price">The maximum price of the items to retrieve.</param>
        /// <returns>A list of ItemDTO objects representing the items with prices less than or equal to the specified price.</returns>
        /// <remarks>If no items are found with prices less than or equal to the specified price, a NotFound response is returned.</remarks>
        /// <exception cref="InvalidOperationException">Thrown if an error occurs while retrieving items from the service.</exception>

        [HttpGet("until-price/{price}")]
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
        /// Retrieves a list of items that have been sold.
        /// </summary>
        /// <returns>A list of ItemDTO objects representing the sold items.</returns>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If an error occurs while retrieving the sold items, a NotFound result is returned with a message describing the error.
        /// </remarks>
        /// <response code="200">The sold items were successfully retrieved.</response>
        /// <response code="404">No sold items were found.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("sold-items")]
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
        /// Retrieves a list of items that have not been sold.
        /// </summary>
        /// <returns>A list of ItemDTO objects representing the unsold items.</returns>
        /// <remarks>
        /// If an error occurs while retrieving the unsold items, a NotFound result is returned with a message describing the error.
        /// </remarks>
        /// <response code="200">The unsold items were successfully retrieved.</response>
        /// <response code="404">No unsold items were found.</response>
        [HttpGet("available-items")]
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
        /// Method to get all items sold by a specific category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// <returns>A list of items sold in the specified category.</returns>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If no items are found in the specified category, a NotFound result is returned.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpGet("sold-items-by-category/{categId}")]
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
        /// Method to get all items available by a specific category.
        /// </summary>
        /// <param name="categId">The ID of the category.</param>
        /// <returns>A list of items available in the specified category.</returns>
        /// <remarks>
        /// If no items are found in the specified category, a NotFound result is returned.
        /// </remarks>
        [HttpGet("available-items-by-category/{categId}")]
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
