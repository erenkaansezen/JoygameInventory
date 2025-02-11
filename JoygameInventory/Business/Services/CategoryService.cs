using JoygameInventory.Business.Interface;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
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

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
                             .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> CreateCategory(Category category)
    {
        _context.Categories.Add(category);
        var result = await _context.SaveChangesAsync();
        return result > 0; 
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        var result = await _context.SaveChangesAsync();
        return result > 0; 
    }

    public async Task<bool> DeleteCategory(int id)
    {
        var category = await GetCategoryByIdAsync(id);
        if (category == null)
        {
            return false;
        }

        _context.Categories.Remove(category);
        var result = await _context.SaveChangesAsync();
        return result > 0; 
    }

    public async Task<List<Category>> SearchCategory(string searchTerm)
    {
        return await _context.Categories
                             .Where(c => c.Name.Contains(searchTerm))
                             .ToListAsync();
    }
    public async Task<IEnumerable<ProductCategory>> GetCategoryProductsAsync(int categoryıd)
    {
        var inventoryAssignments = await _context.ProductCategories
            .Include(ia => ia.Category)
            .Include(ia => ia.Product)  
            .Where(ia => ia.CategoryId == categoryıd) 
            .ToListAsync();

        return inventoryAssignments;
    }
    public async Task<bool> IsCateogryUrlUnique(string CategoryUrl)
    {
        return !await _context.Categories.AnyAsync(s => s.Url == CategoryUrl);
    }
}
