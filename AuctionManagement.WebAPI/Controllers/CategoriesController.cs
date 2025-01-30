using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Implementation;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionManagement.WebAPI.Controllers {
    /// <summary>
    /// Classe que controla as requisições HTTP
    /// </summary>
    [Route("api/[controller]/")]
    [ApiController]
    public class CategoriesController : Controller {

        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService) {
            this.categoriesService = categoriesService;
        }


        /// <summary>
        /// Method that register a new category to the database
        /// </summary>
        /// <param name="itemDTOCreate"></param>
        /// <returns></returns>
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
        /// Method to get all categories
        /// </summary>
        /// <returns></returns>
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
        /// Method to get a category by id
        /// </summary>
        /// <returns></returns>
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
        /// Method that update a category by recieving its id and its JSON body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
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
        /// Method that deletes a category with the id recieved as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                return NotFound(new { message = ex.Message });
            }
        }




        /*
         * ----------------- EXTRA ENDPOINTS -----------------
         */

        /// <summary>
        /// Method to get all categories with items
        /// </summary>
        /// <returns></returns>
        [HttpGet("with-items")]
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
        /// Method to get all categories without items
        /// </summary>
        /// <returns></returns>
        [HttpGet("withoutitems")]
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
