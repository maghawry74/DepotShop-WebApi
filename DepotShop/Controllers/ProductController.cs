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
    public IActionResult Get()
    {
        return Ok(ProductRepository.GetAll());
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult Get(int id)
    {
        var Product = ProductRepository.GetOne(product => product._id == id);
        return (Product != null) ? Ok(Product) : NotFound();
    }

    [HttpPost(Name = "CreateProduct")]
    [Authorize(Policy = "Admin")]
    public IActionResult Post([FromBody] newProductDTO Product)
    {
        if (ModelState.IsValid)
        {
            var newProduct = Mapper.Map<ProductModel>(Product);
            var addedProduct = ProductRepository.Add(newProduct);
            return CreatedAtRoute("CreateProduct", addedProduct);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult Put([FromRoute] int id, [FromBody] EditProductDTO product)
    {
        var EditedProduct = Mapper.Map<ProductModel>(product);
        EditedProduct._id = id;
        var result = ProductRepository.Update(EditedProduct);
        return (result.ModifiedCount > 0) ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "Admin")]
    public IActionResult Delete(int id)
    {
        var result = ProductRepository.Delete(product => product._id == id);
        return (result.DeletedCount > 0) ? Ok() : NotFound();
    }
}
