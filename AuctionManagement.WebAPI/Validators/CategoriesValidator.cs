using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Validators {
    public class CategoriesValidator {
        private readonly AuctionContext context;

        public CategoriesValidator(AuctionContext context) => this.context = context;


        public Category ValidateCategoryExistence(int categId) {
            var category = context.Categories.Find(categId);

            if (category == null) {
                throw new InvalidOperationException("The category id doesn't match any existing category.");
            }

            return category;
        }


        public List<CategoryDTO> ValidateCategoriesList() {
            var categories = context.Categories.ToList();
            var categoriesDTO = new List<CategoryDTO>();

            if (categories == null || !categories.Any()) {
                throw new InvalidOperationException("The categories list is empty.");
            }

            foreach (Category category in categories) {
                categoriesDTO.Add(CategoryDTO.FromCategoryToDTO(category)!);
            }

            return categoriesDTO;
        }


        public void ValidateFilteredList(List<CategoryDTO> categoriesDTO) {
            if (categoriesDTO == null || categoriesDTO.Count == 0) {
                throw new InvalidOperationException("No categories attended to the criterias.");
            }
        }


        public void IsItemsListEmpty(Category category) {
            List<Item> categories = category.Items.ToList();

            if (categories.Count < 1) {
                throw new ArgumentException("Impossible to exclude this category because its items list is not empty.");
            }
        }
    }
}
