using Microsoft.AspNetCore.Mvc;
using Product.Application.Product.Service;
using Features = Product.Application.Product.Features;

namespace Product.Presentation.Product;
[ApiController]
[Route("api/products")]
public class Controller : ControllerBase
{
    private readonly IProductService _productService;

    public Controller(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get([FromRoute] Features.Get.DTO.Request request)
    {
        return Ok(_productService.Get(request));
    }
    [HttpPost]
    public IActionResult Create([FromBody] Features.Create.DTO.Request request)
    {
        return Ok(_productService.Create(request));
    }

}