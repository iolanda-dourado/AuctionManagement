using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {

    /// <summary>
    /// Provides methods for managing categories in the auction system.
    /// </summary>
    public class CategoriesService : ICategoriesService {

        private readonly AuctionContext context;
        private readonly CategoriesValidator categoriesValidator;


        /// <summary>
        /// Initializes a new instance of the CategoriesService class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="categoriesValidator">The validator for categories.</param>
        public CategoriesService(AuctionContext context, CategoriesValidator categoriesValidator) {
            this.context = context;
            this.categoriesValidator = categoriesValidator;
        }


        /// <summary>
        /// Adds a new category to the system.
        /// </summary>
        /// <param name="categDTO">The category to add.</param>
        /// <returns>The added category as a DTO.</returns>
        public CategoryDTO AddCategory(CategoryDTOCreate categDTO) {
            Category category = new Category {
                Description = categDTO.Description
            };
            categoriesValidator.IsCategoryEqualToAnother(category.Description);

            context.Categories.Add(category);
            context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(category)!;
        }


        /// <summary>
        /// Retrieves a list of all categories in the system.
        /// </summary>
        /// <returns>A list of categories as DTOs.</returns>
        public List<CategoryDTO> GetCategories() {
            List<CategoryDTO> categoriesDTO = categoriesValidator.ValidateCategoriesList();

            return categoriesDTO;
        }


        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The category as a DTO, or null if not found.</returns>
        public CategoryDTO GetCategoryById(int id) {
            Category category = categoriesValidator.ValidateCategoryExistence(id);

            return CategoryDTO.FromCategoryToDTO(category)!;
        }


        /// <summary>
        /// Updates an existing category in the system.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="category">The updated category.</param>
        /// <returns>The updated category as a DTO.</returns>
        public CategoryDTO UpdateCategory(int id, Category category) {
            Category existingCateg = categoriesValidator.ValidateCategoryExistence(id);
            categoriesValidator.IsItemsListEmpty(category);
            categoriesValidator.IsCategoryEqualToAnother(category.Description);

            context.Entry(existingCateg).CurrentValues.SetValues(category);
            context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(existingCateg)!;
        }


        /// <summary>
        /// Deletes a category from the system.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>The deleted category as a DTO.</returns>
        public CategoryDTO DeleteCategory(int id) {
            Category category = categoriesValidator.ValidateCategoryExistence(id);
            categoriesValidator.IsItemsListEmpty(category);

            context.Remove(category);
            context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(category!)!;
        }




        /*
         * ------------------------------- EXTRA ENDPOINTS -------------------------------
         */


        /// <summary>
        /// Retrieves a list of categories that have items associated with them.
        /// </summary>
        /// <returns>A list of categories with items as DTOs.</returns>
        public List<CategoryDTO> GetCategoriesWithItems() {
            List<Category> filteredList = context.Categories.Where
                (c => c.Items.Count != 0)
                .Include(c => c.Items)
                .ToList();

            List<CategoryDTO> listDTO = filteredList.ConvertAll(category => CategoryDTO.FromCategoryToDTO(category)!);
            categoriesValidator.ValidateFilteredList(listDTO);

            return listDTO;
        }


        /// <summary>
        /// Retrieves a list of categories that do not have items associated with them.
        /// </summary>
        /// <returns>A list of categories without items as DTOs.</returns>
        public List<CategoryDTO> GetCategoriesWithoutItems() {
            List<Category> filteredList = context.Categories.Where
                (c => c.Items.Count == 0).ToList();
            List<CategoryDTO> listDTO = filteredList.ConvertAll(category => CategoryDTO.FromCategoryToDTO(category)!);

            categoriesValidator.ValidateFilteredList(listDTO);

            return listDTO;
        }
    }

}
