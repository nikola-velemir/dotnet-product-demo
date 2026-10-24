// Ovde je odredjeno preimenovanje namespace-a radi koriscenja namespace sintakse.
using Get = Product.Application.Product.Features.Get;
using Create = Product.Application.Product.Features.Create;

namespace Product.Application.Product.Service;

/// <summary>
/// Ovde je public, jer zelimo da drugi projekti/asembliji pristupe samo apsrakciji
/// </summary>
public interface IProductService
{
    Task<Get.DTO.Response> Get(Get.DTO.Request request);
    Task<Create.DTO.Response> Create(Create.DTO.Request request);
}