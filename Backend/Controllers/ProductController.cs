using Application.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet("{asWishList}")]
        public async Task<ActionResult<List<ProductModel>>> GetAllAsync(bool asWishList,CancellationToken token)
        {
            var model = await productService.GetAllAsync(asWishList,token);

            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> AddOrUpdate([FromBody] ProductModel model ,CancellationToken token)
        {
            var returnModel = await productService.AddOrUpdateAsync(model,token);

            return Ok(returnModel);
        }

        [HttpPost("[action]/{productId}")]
        public async Task<ActionResult<ProductModel>> AddToWishList(Guid productId, CancellationToken token)
        {
             await productService.AddProductToWishList(productId, token);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<ProductModel>>> DeleteAsync(Guid id,CancellationToken token)
        {
           await productService.DeleteAsync(id,token);

            return Ok();
        }
    }
}
