using JoygameInventory.Data.Entities;

namespace JoygameInventory.Business.Interface
{
    public interface ICategoryService 
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<ProductCategory>> GetCategoryProductsAsync(int id);

        Task<List<Category>> SearchCategory(string searchTerm);
        Task<bool> IsCateogryUrlUnique(string CategoryUrl);

    }
}
