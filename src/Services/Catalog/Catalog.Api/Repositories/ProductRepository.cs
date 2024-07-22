
using Catalog.Api.Data;
using System.Xml.Linq;

namespace Catalog.Api.Repositories;

public class ProductRepository : IProductRepository
{
    readonly ICatalogContext _context;

    public ProductRepository(ICatalogContext context)
    {
        _context = context;
    }

    public async Task CreateProduct(Product product)
    => await _context.Products.InsertOneAsync(product);

    public async Task<bool> DeleteProduct(string id)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
        var deletedResult = await _context.Products.DeleteOneAsync(filter);
        return deletedResult.IsAcknowledged && deletedResult.DeletedCount > 0;
    }

    public async Task<Product> GetProduct(string id)
    => await _context.Products
        .Find(x => x.Id.Equals(id))
        .FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetProductByCategory(string category)
    {
        FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Category, category);
        return await _context.Products.Find(filters).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        FilterDefinition<Product> filters = Builders<Product>.Filter.Eq(p => p.Name, name);
        return await _context.Products.Find(filters).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProducts()
    => await _context.Products.Find(x => true).ToListAsync();

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }
}
