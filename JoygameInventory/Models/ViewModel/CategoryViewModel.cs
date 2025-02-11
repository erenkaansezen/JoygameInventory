using JoygameInventory.Data.Entities;

namespace JoygameInventory.Models.ViewModel
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public IEnumerable<ProductCategory> Products { get; set; }

    }
}
