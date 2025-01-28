using AuctionManagement.WebAPI.Data;
using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;
using AuctionManagement.WebAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionManagement.WebAPI.Services.Implementation {
    public class CategoriesService : ICategoriesService {

        private readonly AuctionContext _context;

        public CategoriesService(AuctionContext context) {
            _context = context;
        }

        public CategoryDTO AddCategory(CategoryDTOCreate categDTO) {
            Category category = new Category {
                Description = categDTO.Description
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(category);
        }



        public List<CategoryDTO> GetCategories() {
            var categoriesDTO = new List<CategoryDTO>();

            var categories = _context.Categories
                .Include(c => c.Items)
                .ToList();

            if (categories != null && categories.Any()) {
                foreach (Category categ in categories)
                    categoriesDTO.Add(CategoryDTO.FromCategoryToDTO(categ));

                return categoriesDTO;
            }

            return null;
        }

        public CategoryDTO GetCategoryById(int id) {
            Category category = _context.Categories.Find(id)!;

            if (category == null)
                return null!;

            return CategoryDTO.FromCategoryToDTO(category)!;
        }

        public CategoryDTO UpdateCategory(int id, Category category) {
            var existingCateg = _context.Categories.Find(id);
            if (existingCateg == null) return null!;

            _context.Entry(existingCateg).CurrentValues.SetValues(category);
            _context.SaveChanges();

            return CategoryDTO.FromCategoryToDTO(existingCateg)!;
        }


        public CategoryDTO DeleteCategory(int id) {
            Category category = _context.Categories.Find(id)!;

            if (category != null) {
                _context.Remove(category);
                _context.SaveChanges();
            }

            return CategoryDTO.FromCategoryToDTO(category!)!;
        }
    }
}
