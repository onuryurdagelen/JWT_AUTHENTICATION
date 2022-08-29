using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AuthServer.Core.DTO;
using AuthServer.Core.Entity;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product,ProductDto> _productService;

        public ProductController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
                return ActionResultInstance(await _productService.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _productService.AddAsync(productDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _productService.UpdateAsync(productDto,productDto.Id));
        }

        [HttpDelete("{id}")]
        //api/products?id=2
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _productService.RemoveAsync(id));
        }
    }
}