namespace JoygameInventory.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty; // Telefon => telefon || Beyaz Eşya => beyaz-esya

        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
