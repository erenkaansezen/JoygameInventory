

using JoygameInventory.Data.Context;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Components
{
    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly InventoryContext _context;


        public CategoriesListViewComponent(InventoryContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke(string viewName)
        {
            // Eğer viewName null veya boşsa, varsayılan olarak "default" kullan
            viewName = string.IsNullOrEmpty(viewName) ? "Default" : viewName;

            // ViewBag.SelectedCategory'yi RouteData'dan al
            ViewBag.SelectedCategory = RouteData?.Values["category"];

            // Kategorileri al ve View'a ilet
            var categories = _context.Categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Url = c.Url
            }).ToList();

            // Dinamik olarak belirlenen view'ı render et
            return View(viewName, categories);
        }
    }
}
