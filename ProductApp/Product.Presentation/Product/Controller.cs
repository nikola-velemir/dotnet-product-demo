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
    public async Task<IActionResult> Get([FromRoute] Features.Get.DTO.Request request)
    {
        return Ok(await _productService.Get(request));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Features.Create.DTO.Request request)
    {
        return Ok(await _productService.Create(request));
    }

    [HttpGet("byName")]
    public async Task<IActionResult> GetByName([FromQuery] Features.GetByName.DTO.Request request)
    {
        var response = await _productService.GetByName(request);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] Features.Update.DTO.Request.Body content)
    {
        var request = new Features.Update.DTO.Request(id, content);
        var response = await _productService.Update(request);
        return Ok(response);
    }
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        [FromRoute]
        Features.Delete.DTO.Request request)
    {
        var response = await _productService.Delete(request);
        return Ok(response);
    }
}