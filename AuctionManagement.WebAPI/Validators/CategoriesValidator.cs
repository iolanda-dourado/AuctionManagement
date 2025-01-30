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


        public void IsCategoryEqualToAnother(string categDesc) {
            var categories = context.Categories.ToList();
            foreach (Category cat in categories) {
                if (cat.Description.Equals(categDesc)) {
                    throw new ArgumentException("The category already exists. Try again with another description.");
                }
            }
        }


        public List<CategoryDTO> ValidateCategoriesList() {
            var categories = context.Categories.ToList();
            var categoriesDTO = new List<CategoryDTO>();

            if (categories == null || !categories.Any()) {
                throw new InvalidOperationException("The items list is empty.");
            }

            foreach (Category category in categories) {
                categoriesDTO.Add(CategoryDTO.FromCategoryToDTO(category)!);
            }

            return categoriesDTO;
        }


        public void ValidateFilteredList(List<CategoryDTO> categoriesDTO) {
            if (categoriesDTO == null || categoriesDTO.Count == 0) {
                throw new InvalidOperationException("No items attended to the criterias.");
            }
        }


        public void IsItemsListEmpty(Category category) {
            List<Item> items = category.Items.ToList();

            if (items.Count != 0) {
                throw new ArgumentException("Impossible to conclude this action because the category items list is not empty.");
            }
        }
    }
}
