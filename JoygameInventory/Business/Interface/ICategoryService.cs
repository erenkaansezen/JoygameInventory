using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;

namespace JoygameInventory.Business.Interface
{
    public interface ICategoryService 
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesAsync(string searchTerm);
        Task<CategoryViewModel> GetCategoryDetailsAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategory(CategoryViewModel model);
        Task<bool> UpdateCategory(CategoryViewModel model);
        Task<bool> DeleteCategory(int categoryId);
        Task<IEnumerable<ProductCategory>> GetCategoryProductsAsync(int id);
        Task<bool> IsCateogryUrlUnique(string CategoryUrl);

    }
}
