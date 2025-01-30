using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Enums;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using AuctionManagement.WebAPI.Validators;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class CategoriesService : ICategoriesService {

        private readonly AuctionContext context;
        private readonly CategoriesValidator categoriesValidator;

        public CategoriesService(AuctionContext context, CategoriesValidator categoriesValidator) {
            this.context = context;
            this.categoriesValidator = categoriesValidator;
        }

        public CategoryDTO AddCategory(CategoryDTOCreate categDTO) {
            Category category = new Category {
                Description = categDTO.Description
            };
            categoriesValidator.IsCategoryEqualToAnother(category.Description);

            context.Categories.Add(category);
            context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(category)!;
        }


        public List<CategoryDTO> GetCategories() {
            List<CategoryDTO> categoriesDTO = categoriesValidator.ValidateCategoriesList();

            return categoriesDTO;
        }


        public CategoryDTO GetCategoryById(int id) {
            Category category = categoriesValidator.ValidateCategoryExistence(id);

            return CategoryDTO.FromCategoryToDTO(category)!;
        }

        public CategoryDTO UpdateCategory(int id, Category category) {
            Category existingCateg = categoriesValidator.ValidateCategoryExistence(id);
            categoriesValidator.IsItemsListEmpty(category);
            categoriesValidator.IsCategoryEqualToAnother(category.Description);

            context.Entry(existingCateg).CurrentValues.SetValues(category);
            context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(existingCateg)!;
        }


        public CategoryDTO DeleteCategory(int id) {
            Category category = categoriesValidator.ValidateCategoryExistence(id);
            categoriesValidator.IsItemsListEmpty(category);

            context.Remove(category);
            context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(category!)!;
        }


        public List<CategoryDTO> GetCategoriesWithItems() {
            List<Category> filteredList = context.Categories.Where
                (c => c.Items.Count == 0)
                .Include(c => c.Items)
                .ToList();

            List<CategoryDTO> listDTO = filteredList.ConvertAll(category => CategoryDTO.FromCategoryToDTO(category)!);
            categoriesValidator.ValidateFilteredList(listDTO);

            return listDTO;
        }


        public List<CategoryDTO> GetCategoriesWithoutItems() {
            List<Category> filteredList = context.Categories.Where
                (c => c.Items.Count > 0).ToList();
            List<CategoryDTO> listDTO = filteredList.ConvertAll(category => CategoryDTO.FromCategoryToDTO(category)!);

            categoriesValidator.ValidateFilteredList(listDTO);

            return listDTO;
        }
    }

}
