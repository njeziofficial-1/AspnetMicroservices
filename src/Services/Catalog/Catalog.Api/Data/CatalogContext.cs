namespace Catalog.Api.Data;

public class CatalogContext : ICatalogContext
{
    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        CatalogContextSeed.SeedData(Products);
    }
    public IMongoCollection<Product> Products { get; }
}

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> products)
    {
        bool isProductsExist = products.Find(x => true).Any();
        if (!isProductsExist)
        {
            products.InsertManyAsync(GetPreConfiguredProducts());
        }
    }

    private static IEnumerable<Product> GetPreConfiguredProducts()
    => new[]
    {
        new Product
        {
            Id = "602d2149e773f2a3990b47f1",
            Name = "iPhone X",
            Summary = "The best iPhone of its time",
            Description = "Very good phone",
            ImageFile = "product-1.png",
            Price = 950.08,
            Category = "Smart Phone"
        },
         new Product
         {
            Id = "602d2149e773f2a3990b47f2",
            Name = "Samsung S24",
            Summary = "The best Android of its time",
            Description = "Very good phone",
            ImageFile = "product-2.png",
            Price = 1550.08,
            Category = "Smart Phone"
         },
         new Product
        {
            Id = "602d2149e773f2a3990b47f3",
            Name = "Huawei Y9S",
            Summary = "The best Huawei Android of its time",
            Description = "Very good phone",
            ImageFile = "product-3.png",
            Price = 550.08,
            Category = "Smart Phone"
        },
         new Product
         {
            Id = "602d2149e773f2a3990b47f4",
            Name = "Toyota Highlander",
            Summary = "The best car of its time",
            Description = "Very good car",
            ImageFile = "product-4.png",
            Price = 34550.08,
            Category = "Vehicle"
         },
         new Product
        {
            Id = "602d2149e773f2a3990b47f5",
            Name = "Lexus",
            Summary = "The best car of its time",
            Description = "Very good car",
            ImageFile = "product-5.png",
            Price = 45950.08,
            Category = "Vehicle"
        },
         new Product
         {
            Id = "602d2149e773f2a3990b47f6",
            Name = "LG Washing Machine",
            Summary = "A great device to assist",
            Description = "A semi automatic washing machine",
            ImageFile = "product-6.png",
            Price = 750.08,
            Category = "Home Appliance"
         }
    };
}
