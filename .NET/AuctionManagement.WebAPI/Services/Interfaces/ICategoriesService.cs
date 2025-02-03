using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Services.Interfaces {

    /// <summary>
    /// Defines the interface for category services.
    /// </summary>
    public interface ICategoriesService {

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="categoryDTO">A CategoryDTOCreate object containing the details of the category to add.</param>
        /// <returns>A CategoryDTO object representing the newly added category.</returns>
        public CategoryDTO AddCategory(CategoryDTOCreate categoryDTO);


        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A list of CategoryDTO objects.</returns>
        public List<CategoryDTO> GetCategories();

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>A CategoryDTO object representing the category with the specified ID.</returns>
        public CategoryDTO GetCategoryById(int id);

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="category">A Category object containing the updated details of the category.</param>
        /// <returns>A CategoryDTO object representing the updated category.</returns>
        public CategoryDTO UpdateCategory(int id, Category category);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A CategoryDTO object representing the deleted category.</returns>
        public CategoryDTO DeleteCategory(int id);

        /// <summary>
        /// Retrieves a list of categories that contains items.
        /// </summary>
        /// <returns>A list of CategoryDTO objects representing categories with items.</returns>
        public List<CategoryDTO> GetCategoriesWithItems();

        // <summary>
        /// Retrieves a list of categories that do not contains items.
        /// </summary>
        /// <returns>A list of CategoryDTO objects representing categories without items.</returns>
        public List<CategoryDTO> GetCategoriesWithoutItems();
    }
}
