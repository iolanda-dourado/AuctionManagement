using AuctionManagement.WebAPI.Dtos;
using AuctionManagement.WebAPI.Models;

namespace AuctionManagement.WebAPI.Services.Interfaces {
    public interface ICategoriesService {

        public List<CategoryDTO> GetCategories();

        public CategoryDTO GetCategoryById(int id);

        public CategoryDTO AddCategory(CategoryDTOCreate categoryDTO);

        public CategoryDTO UpdateCategory(int id, Category category);

        public CategoryDTO DeleteCategory(int id);
    }
}
