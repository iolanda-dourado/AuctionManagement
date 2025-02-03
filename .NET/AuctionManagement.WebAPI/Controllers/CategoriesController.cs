using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Implementation;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {
    /// <summary>
    /// Controller for managing categories in the auction management system.
    /// Provides endpoints for adding, retrieving, updating, and deleting categories.
    /// </summary>
    /// <remarks>
    /// This controller requires authorization with the Admin role for most endpoints.
    /// </remarks>
    [Route("api/[controller]/")]
    [ApiController]
    public class CategoriesController : Controller {

        /// <summary>
        /// Service for categories
        /// </summary>
        private readonly ICategoriesService categoriesService;

        /// <summary>
        /// Constructor for the CategoriesController
        /// </summary>
        /// <param name="categoriesService"></param>
        public CategoriesController(ICategoriesService categoriesService) {
            this.categoriesService = categoriesService;
        }


        /// <summary>
        /// Method to add a new category to the database. Requires authorization with Admin role.
        /// </summary>
        /// <param name="categoryDTOCreate">The category to be added</param>
        /// <returns>The added category. Returns NotFound if the category cannot be added due to invalid operation, 
        /// or BadRequest if the category cannot be added due to invalid argument.</returns>

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<IEnumerable<CategoryDTO>> Add(CategoryDTOCreate categoryDTOCreate) {
            try {
                var createdCategory = categoriesService.AddCategory(categoryDTOCreate);
                return CreatedAtAction(nameof(Add), new { id = createdCategory.Id }, createdCategory);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to retrieve all categories from the database.
        /// </summary>
        /// <returns>A list of CategoryDTO objects representing the categories in the database.</returns>
        /// <remarks>
        /// If an error occurs while retrieving the categories, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories() {
            try {
                var categoriesDTO = categoriesService.GetCategories();

                return Ok(categoriesDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }

        }


        /// <summary>
        /// Method to retrieve a category by its id from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be retrieved.</param>
        /// <returns>A CategoryDTO object representing the category with the specified id, or a NotFound result 
        /// if the category is not found.</returns>
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CategoryDTO>> GetById(int id) {
            try {
                CategoryDTO category = categoriesService.GetCategoryById(id);
                return Ok(category);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Method to update a category in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be updated.</param>
        /// <param name="category">The updated category data.</param>
        /// <returns>An ActionResult containing the updated CategoryDTO instance if successful, otherwise a NotFound 
        /// result with a descriptive message.</returns>
        /// <response code="200">The category was successfully updated.</response>
        /// <response code="404">The category was not found.</response>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If an error occurs while updating the category, a NotFound or Bad Request result is returned with a message describing the error.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, Category category) {
            try {
                CategoryDTO updatedCateg = categoriesService.UpdateCategory(id, category);

                return Ok(updatedCateg);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Deletes a category from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the category to be deleted.</param>
        /// <returns>An ActionResult containing a NoContent result if successful, otherwise a NotFound result with a descriptive message.</returns>
        /// <response code="204">The category was successfully deleted.</response>
        /// <response code="404">The category was not found.</response>
        /// <remarks>
        /// This method requires authorization with the Admin role.
        /// If an error occurs while deleting the category, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id) {
            try {
                categoriesService.DeleteCategory(id);
                return NoContent();
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex) {
                return BadRequest(new { message = ex.Message });
            }
        }




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */

        /// <summary>
        /// Method to retrieve all categories that contain items from the database.
        /// </summary>
        /// <returns>A list of CategoryDTO objects representing the categories with items in the database.</returns>
        /// <remarks>
        /// If an error occurs while retrieving the categories, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [HttpGet("contain-items")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesWithItems() {
            try {
                List<CategoryDTO> categoriesDTO = categoriesService.GetCategoriesWithItems();

                return Ok(categoriesDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }



        /// <summary>
        /// Method to retrieve all categories that do not contain items from the database.
        /// </summary>
        /// <returns>A list of CategoryDTO objects representing the categories without items in the database.</returns>
        /// <remarks>
        /// If an error occurs while retrieving the categories, a NotFound result is returned with a message describing the error.
        /// </remarks>
        [HttpGet("dont-contain-items")]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategoriesWithoutItems() {
            try {
                List<CategoryDTO> categoriesDTO = categoriesService.GetCategoriesWithoutItems();

                return Ok(categoriesDTO);
            }
            catch (InvalidOperationException ex) {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
