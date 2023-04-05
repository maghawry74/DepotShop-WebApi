using AutoMapper;
using DepotShopDataAccess.Repository;
using DepotShopModels.DTOs.Product;
using DepotShopModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace DepotShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    public IProductRepository ProductRepository { get; }
    public IMapper Mapper { get; }

    public ProductController(IProductRepository productRepository, IMapper mapper)
    {
        ProductRepository = productRepository;
        Mapper = mapper;
    }
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        return Ok(await ProductRepository.GetAll());
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int id)
    {
        var Product = await ProductRepository.GetOne(product => product._id == id);
        return (Product != null) ? Ok(Product) : NotFound();
    }

    [HttpPost(Name = "CreateProduct")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Post([FromBody] newProductDTO Product)
    {
        if (ModelState.IsValid)
        {
            var newProduct = Mapper.Map<ProductModel>(Product);
            var addedProduct = await ProductRepository.Add(newProduct);
            return CreatedAtRoute("CreateProduct", addedProduct);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] EditProductDTO product)
    {
        var EditedProduct = Mapper.Map<ProductModel>(product);
        EditedProduct._id = id;
        var result = await ProductRepository.Update(EditedProduct);
        return (result.ModifiedCount > 0) ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await ProductRepository.Delete(product => product._id == id);
        return (result.DeletedCount > 0) ? Ok() : NotFound();
    }
}
