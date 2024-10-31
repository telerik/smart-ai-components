namespace AIDataGridSemanticSearch;

public static class ProductService
{
    public static List<Product> CreateData()
    {
        List<Product> data = new List<Product>
        {
            new Product { CategoryId = 1, CategoryName = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" },
            new Product { CategoryId = 2, CategoryName = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings" },
            new Product { CategoryId = 3, CategoryName = "Confections",  Description = "Desserts, candies, and sweet breads" },
            new Product { CategoryId = 4, CategoryName = "Dairy Products", Description = "Cheeses" },
            new Product { CategoryId = 5, CategoryName = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" },
            new Product { CategoryId = 6, CategoryName = "Meat/Poultry", Description = "Prepared meats" },
            new Product { CategoryId = 7, CategoryName = "Produce", Description = "Dried fruit and bean curd" },
            new Product { CategoryId = 8, CategoryName = "Seafood", Description = "Seaweed and fish" },
        };

        return data;
    }
}