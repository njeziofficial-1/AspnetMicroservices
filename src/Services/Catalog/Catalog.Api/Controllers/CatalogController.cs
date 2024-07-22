using Catalog.Api.Repositories;
using System.Net;

namespace Catalog.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    readonly IProductRepository _repository;
    readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts()
        => Ok(await _repository.GetProducts());

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProductId(string id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
        {
            _logger.LogError($"Product with id:{id}, not found");
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet]
    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProductByCategory(string category)
    {
        var product = await _repository.GetProductByCategory(category);
        return Ok(product);
    }

    [HttpGet]
    [Route("[action]/{name}", Name = "GetProductByName")]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProductByName(string name)
    {
        var product = await _repository.GetProductByName(name);
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        await _repository.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    => Ok(await _repository.UpdateProduct(product));


    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<IActionResult>DeleteProductById(string id)
        => Ok(await _repository.DeleteProduct(id));
}
