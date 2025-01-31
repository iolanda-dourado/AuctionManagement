using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Validators {

    /// <summary>
    /// Validates categories based on various criteria.
    /// </summary>
    public class CategoriesValidator {
        private readonly AuctionContext context;

        /// <summary>
        /// Initializes a new instance of the CategoriesValidator class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CategoriesValidator(AuctionContext context) => this.context = context;


        /// <summary>
        /// Validates the existence of a category by its ID.
        /// </summary>
        /// <param name="categId">The ID of the category to validate.</param>
        /// <returns>The category if it exists.</returns>
        /// <exception cref="InvalidOperationException">If the category does not exist.</exception>
        public Category ValidateCategoryExistence(int categId) {
            var category = context.Categories.Find(categId);

            if (category == null) {
                throw new InvalidOperationException("The category id doesn't match any existing category.");
            }

            return category;
        }


        /// <summary>
        /// Checks if a category with the same description already exists.
        /// </summary>
        /// <param name="categDesc">The description of the category to check.</param>
        /// <exception cref="ArgumentException">If a category with the same description already exists.</exception>
        public void IsCategoryEqualToAnother(string categDesc) {
            var categories = context.Categories.ToList();
            foreach (Category cat in categories) {
                if (cat.Description.Equals(categDesc)) {
                    throw new ArgumentException("The category already exists. Try again with another description.");
                }
            }
        }


        /// <summary>
        /// Validates the list of categories and returns a list of category DTOs.
        /// </summary>
        /// <returns>A list of category DTOs.</returns>
        /// <exception cref="InvalidOperationException">If the list of categories is empty.</exception>
        public List<CategoryDTO> ValidateCategoriesList() {
            var categories = context.Categories.ToList();
            var categoriesDTO = new List<CategoryDTO>();

            if (categories == null || categories.Count == 0) {
                throw new InvalidOperationException("The items list is empty.");
            }

            foreach (Category category in categories) {
                categoriesDTO.Add(CategoryDTO.FromCategoryToDTO(category)!);
            }

            return categoriesDTO;
        }


        /// <summary>
        /// Validates a filtered list of category DTOs.
        /// </summary>
        /// <param name="categoriesDTO">The list of category DTOs to validate.</param>
        /// <exception cref="InvalidOperationException">If the list is empty.</exception>
        public void ValidateFilteredList(List<CategoryDTO> categoriesDTO) {
            if (categoriesDTO == null || categoriesDTO.Count == 0) {
                throw new InvalidOperationException("No items attended to the criterias.");
            }
        }


        /// <summary>
        /// Checks if the items list of a category is empty.
        /// </summary>
        /// <param name="category">The category to check.</param>
        /// <exception cref="ArgumentException">If the items list is not empty.</exception>
        public void IsItemsListEmpty(Category category) {
            List<Item> items = category.Items.ToList();

            if (items.Count != 0) {
                throw new ArgumentException("Impossible to conclude this action because the category items list is not empty.");
            }
        }
    }
}
