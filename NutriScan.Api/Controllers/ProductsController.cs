using Microsoft.AspNetCore.Mvc;
using NutriScan.Api.DTOs;
using NutriScan.Api.Models;
using NutriScan.Api.Services;

namespace NutriScan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{barcode}")]
    public async Task<ActionResult<ProductDTO>> GetByBarcode(string barcode)
    {
        var product = await _productService.GetProductAsync(barcode);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(new ProductDTO(
            product.Barcode,
            product.Name,
            product.Brand,
            product.ImageUrl,
            product.Nutrients,
            product.Benefits,
            product.Malefits,
            product.HealthScore,
            product.Source,
            product.LastUpdated
        ));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDTO>> Create(Product product)
    {
        var created = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetByBarcode), new { barcode = created.Barcode }, new ProductDTO(
            created.Barcode,
            created.Name,
            created.Brand,
            created.ImageUrl,
            created.Nutrients,
            created.Benefits,
            created.Malefits,
            created.HealthScore,
            created.Source,
            created.LastUpdated
        ));
    }
}
