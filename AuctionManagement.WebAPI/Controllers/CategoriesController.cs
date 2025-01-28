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
    public class CategoriesController : Controller {

        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService) {
            _categoriesService = categoriesService;
        }

        /// <summary>
        /// Method to get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories() {
            var categoriesDTO = _categoriesService.GetCategories();

            if (categoriesDTO == null || !categoriesDTO.Any()) {
                return NoContent();
            }

            return Ok(_categoriesService.GetCategories());
        }

        /// <summary>
        /// Method to get a category by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<CategoryDTO> GetById(int id) {

            if (_categoriesService == null)
                return NotFound();

            CategoryDTO category = _categoriesService.GetCategoryById(id);

            if (category == null)
                return NotFound();

            return Ok(category);
        }


        /// <summary>
        /// Method that register a new category to the database
        /// </summary>
        /// <param name="itemDTOCreate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<CategoryDTO> Add(CategoryDTOCreate categoryDTOCreate) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState); // Retorna erros de validação
            }

            var createdCategory = _categoriesService.AddCategory(categoryDTOCreate);

            return CreatedAtAction(nameof(Add), new { id = createdCategory.Id }, createdCategory);
        }



        /// <summary>
        /// Method that update a category by recieving its id and its JSON body
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult Update(int id, Category category) {

            if (category.Id != id) {
                return BadRequest();
            }

            CategoryDTO updatedCateg = _categoriesService.UpdateCategory(id, category);

            return Ok(updatedCateg);
        }


        /// <summary>
        /// Method that deletes a category with the id recieved as parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            if (_categoriesService == null)
                return NoContent();

            CategoryDTO categoryDTO = _categoriesService.GetCategoryById(id);

            if (categoryDTO == null)
                return NotFound();

            _categoriesService.DeleteCategory(id);
            return NoContent();
        }
    }
}
