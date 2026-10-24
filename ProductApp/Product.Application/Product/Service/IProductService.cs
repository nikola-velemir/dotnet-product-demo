// Ovde je odredjeno preimenovanje namespace-a radi koriscenja namespace sintakse.

using Get = Product.Application.Product.Features.Get;
using Create = Product.Application.Product.Features.Create;
using GetByName = Product.Application.Product.Features.GetByName;
using Delete = Product.Application.Product.Features.Delete;
using Update = Product.Application.Product.Features.Update;
namespace Product.Application.Product.Service;

/// <summary>
/// Ovde je public, jer zelimo da drugi projekti/asembliji pristupe samo apsrakciji
/// </summary>
public interface IProductService
{
    Task<List<GetByName.DTO.Response>> GetByName(GetByName.DTO.Request request);
    Task<Get.DTO.Response> Get(Get.DTO.Request request);
    Task<Create.DTO.Response> Create(Create.DTO.Request request);
    Task<Delete.DTO.Response> Delete(Delete.DTO.Request request);
    Task<Update.DTO.Response> Update(Update.DTO.Request request);
}