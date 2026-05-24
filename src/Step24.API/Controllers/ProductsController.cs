using Microsoft.AspNetCore.Mvc;
using Step022.Application.Products;

namespace Step24.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_productService.GetAllProducts());

        }


        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProduct(id);
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(string name, decimal price, int stockQuantity)
        {
            var product = _productService.CreateProduct(name, price, stockQuantity);
            return Ok(product);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, string name, decimal price)
        {
            var product = _productService.UpdateProduct(id, name, price);
            return Ok(product);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
