using System.Text.Json;
using ECommerceStore.Api.Data;
using ECommerceStore.Api.Entities;
using ECommerceStore.Api.Extensions;
using ECommerceStore.Api.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceStore.Api.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly StoreContext _storeContext;

        public ProductsController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProductsAsync(
            [FromQuery] ProductParams productParams)
        {
            var query = _storeContext.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query, productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFiltersAsync()
        {
            var brands = await _storeContext.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _storeContext.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _storeContext.Products.FindAsync(id);

            if (product is null)
                return NotFound();

            return product;
        }
    }
}