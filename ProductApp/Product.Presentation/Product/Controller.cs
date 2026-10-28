using Microsoft.AspNetCore.Mvc;
using Product.Application.Product.Service;
using Features = Product.Application.Product.Features;

namespace Product.Presentation.Product;

//Putem ovog atributa deklarisem da je klasa kontroler
[ApiController]
//Zadajem baznu rutu
[Route("api/products")]
public class Controller : ControllerBase
{
    private readonly IProductService _productService;

    public Controller(IProductService productService)
    {
        _productService = productService;
    }

    //Ovaj enpoint je pogodjen ako se pogodi http://localhost:xxxx/api/products/neki_id sa GET
    [HttpGet("{id:guid}")]
    // AspNetCore moze automatski da prepakuje parametar id u objekat Request putem atributa [FromRoute]
    // On ce iz rute iscupati ID i kreirati objekat.
    public async Task<IActionResult> Get([FromRoute] Features.Get.DTO.Request request)
    {
        return Ok(await _productService.Get(request));
    }
    //Ovaj enpoint je pogodjen ako se pogodi http://localhost:xxxx/api/products sa POST
    [HttpPost]
    // AspNetCore ce automatksi mapirati JSON sadrzaj iz zahteva u Request putem atributa [FromBody]
    public async Task<IActionResult> Create([FromBody] Features.Create.DTO.Request request)
    {
        return Ok(await _productService.Create(request));
    }

    [HttpGet("byName")]
    // AspNetCore ce iz URL iscupati query parametre i napraviti Request objekat.
    public async Task<IActionResult> GetByName([FromQuery] Features.GetByName.DTO.Request request)
    {
        var response = await _productService.GetByName(request);
        return Ok(response);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        // Ovde AspNetCore ne moze sve da upakuje u Request, stoga putem [FromRoute] izvucemo Id,
        // pa putem [FromBody] uzmemo ono sto je u telu http zahteva i u metodi kreiramo request objekat koji damo servisu.
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