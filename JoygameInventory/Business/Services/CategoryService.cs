using JoygameInventory.Business.Interface;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly InventoryContext _context;


    public CategoryService(InventoryContext context)
    {
        _context = context;

    }
    public async Task<IEnumerable<Category>> GetCategoriesAsync(string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return await GetAllCategoriesAsync();  
        }
        return await SearchCategory(searchTerm);
    }
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<CategoryViewModel> GetCategoryDetailsAsync(int id)
    {
        var category = await GetCategoryByIdAsync(id);
        var categoryProducts = await GetCategoryProductsAsync(category.Id);
        var model = new CategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Url = category.Url,
            Products = categoryProducts
        };
        return model;
    }



    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
                             .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> CreateCategory(CategoryViewModel model)
    {
        var category = new Category
        {
            Name = model.Name,
            Url = model.Url,
        };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return true; 
    }

    public async Task<bool> UpdateCategory(CategoryViewModel model)
    {
        var category = await GetCategoryByIdAsync(model.Id);
        if (category != null)
        {
            category.Name = model.Name;
            category.Url = model.Url;

            _context.Update(category);
            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCategory(int categoryId)
    {
        var category = await GetCategoryByIdAsync(categoryId);
        if (category != null)
        {
            _context.Categories.Remove(category); 
            await _context.SaveChangesAsync(); 
            return true; 
        }
        return false; 
    }


    public async Task<List<Category>> SearchCategory(string searchTerm)
    {
        return await _context.Categories
                             .Where(c => c.Name.Contains(searchTerm))
                             .ToListAsync();
    }
    public async Task<IEnumerable<ProductCategory>> GetCategoryProductsAsync(int categoryId)
    {
        var inventoryAssignments = await _context.ProductCategories
            .Include(ia => ia.Category)
            .Include(ia => ia.Product)  
            .Where(ia => ia.CategoryId == categoryId) 
            .ToListAsync();

        return inventoryAssignments;
    }
    public async Task<bool> IsCateogryUrlUnique(string CategoryUrl)
    {
        return !await _context.Categories.AnyAsync(s => s.Url == CategoryUrl);
    }
}
