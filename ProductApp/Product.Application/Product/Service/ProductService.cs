// Ovde je odredjeno preimenovanje namespace-a radi koriscenja namespace sintakse.

using Microsoft.EntityFrameworkCore;
using Get = Product.Application.Product.Features.Get;
using Create = Product.Application.Product.Features.Create;
using GetByName = Product.Application.Product.Features.GetByName;
using Product.Application.Common.Db;
using Product.Application.Common.Logs.Service;
using Delete = Product.Application.Product.Features.Delete;
using Update = Product.Application.Product.Features.Update;

namespace Product.Application.Product.Service;

/// <summary>
/// Ovde je internal, jer NE zelimo da drugi projekti/asembliji pristupe implementaciji
/// Java nema slican mehanizam. Jedino nalik je "package-private", odnosno da konkretna klasa
/// moze biti pristupana samo iz istog paketa.
/// </summary>
internal class ProductService : IProductService
{
    private readonly IDbContext _dbContext;
    private readonly ILogService _logService;

    public ProductService(IDbContext dbContext, ILogService logService)
    {
        _dbContext = dbContext;
        _logService = logService;
    }


    // U .NET koristimo async kljucnu rec i klase Task, iz razloga sto zelimo da omogucimo konkurentan pristup korisnika.
    // Ukoliko to ne koristimo, svaki poziv serveru blokira druge korisnike.
    public async Task<Get.DTO.Response> Get(Get.DTO.Request request)
    {
        var id = request.Id;
        // Intefejs QUERYABLE vam dozvoljavam da napraviti specifikaciju SQL querija u kodu.
        // Kada ga napravite, query se ne salje odmah u bazu,
        // potrebno je nakon toga pozvati neku operaciju koja ga "TERMINIRA".
        // Neke od takvih operacija su FirstOrdDefault(Async), ToList(Async), Count(Async),...
        IQueryable<Domain.Product.Entity.Product> queryable = _dbContext.Products.Where(product => product.Id == id);


        // AsNoTracking optimizacija gasi provere da li u naknadnom kodu ima promena na entitetu.
        // Ako se ne ugasi, Komponenta EntityFrameWorka zaduzena za pracene promena (DirtyChecker valjda :P) ce smatrati da se moze desiti modifkacija objekta,
        // pa da mora biti perzistirana u bazu podataka.
        // Za querije koje su singularno dobavljanja, filtriranje i pretraga, koristite AsNoTracking.
        queryable = queryable.AsNoTracking();

        // Da ni bismo imali lazy ucitavanje, koristimo Include. Include ce dodano povuci u iz baze varijante. Ovo ce biti objedinjeno u jednom SQL upitu.
        // Spring ima slican mehanziam, ali je neophodno obratiti paznju da li se entiteti ucitavju eager ili lazy.
        // Ovo vam je primer eager ucitavanja.
        // Da smo radili lazy, nako nsto bismo pristupili listu u objektu (govorim za Spring), morao bi bi da se posalje jos jedan upit ka bazi.
        // Ovo dovodi do N + 1 problema.

        queryable = queryable.Include(product => product.Variants);

        // FirstOrDefault(Async) pronalazi prvi red u bazi koji je specificran putem Queryable,
        // ili lambde unutar zagrada same metode (FirstOrDefaultAsync(x=>x.parametar == mojParametar)).
        // Ako ga pronadje, vrati objekat, u suprotnom vraca null.
        Domain.Product.Entity.Product? product = await queryable.FirstOrDefaultAsync();

        if (product == null)
            throw new Exception(
                "Not found!"); // ovde inace vratite smislenu gresku sa smislenim tipom, koju excpetion handler middleware uhvati.

        // Prepakujemo domenski objekat u DTO i vratimo.
        Get.DTO.Response response = new Get.DTO.Response(
            product.Id,
            product.Name,
            product.Description,
            product.Variants.Select(variant => new Get.DTO.Response.Variant(
                variant.Name,
                variant.Price)).ToList()
        );
        // Vratimo rezultat
        return response;
    }

    public Task<List<GetByName.DTO.Response>> GetByName(GetByName.DTO.Request request)
    {
        var name = request.Name;

        // Ovde nije potrebno da radimo Include, kako usecase ne zahteva join na Variants tabelu.
        var queryable = _dbContext.Products.Where(product => product.Name == name);

        queryable = queryable.AsNoTracking();

        var response = queryable
            .Select(product => new GetByName.DTO.Response(
                    product.Id,
                    product.Name,
                    product.Description
                )
            ).ToListAsync();

        return response;
    }

    public async Task<Create.DTO.Response> Create(Create.DTO.Request request)
    {
        var name = request.Name;
        var description = request.Description;

        Domain.Product.Entity.Product product
            = Domain.Product.Entity.Product.Create(name, description);

        // Ovde ne zelimo async, zelimo da blokiramo prilikom dodavanja.
        _dbContext.Products.Add(product);

        // Ovde cekamo da se zavrsi perzistencija u bazu, Istrazite sta je UNIT OF WORK obrazac,
        // i cemu on sluzi kada budemo pricali o tranzakcijama.
        await _dbContext.SaveChangesAsync();
        return new Create.DTO.Response(
            product.Id,
            product.Name,
            product.Description);
    }

    public async Task<Delete.DTO.Response> Delete(Delete.DTO.Request request)
    {
        var id = request.id;
        await using var tx = await _dbContext.Database.BeginTransactionAsync();

        var product = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);
        if (product == null) throw new Exception("Not found");

        _dbContext.Products.Remove(product);
        await _logService.Create($"Product {product.Name} has been deleted");

        await tx.CommitAsync();

        return new Delete.DTO.Response(product.Id);
    }
    
    public async Task<Update.DTO.Response> Update(Update.DTO.Request request)
    {
        var id = request.Id;
        var content = request.Content;
        await using var tx = await _dbContext.Database.BeginTransactionAsync();
    
        var product = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);
        if (product == null) throw new Exception("Not found");
    
        product.Description = content.Description;
        product.Name = content.Name;
    
        await _logService.Create($"Product {product.Id} has been updated");
        await _dbContext.SaveChangesAsync();
        await tx.CommitAsync();
    
        return new Update.DTO.Response(product.Id, product.Name, product.Description);
    }
    //Ovaj primer bi bacio gresku, metoda LoggingService-a provera da
    // li postoji transakcija kojoj treba da se prikljuci da bi upisao log.
    // Kako ne postoji i jedna otpoceta transakcija, bacice exception.
    // Ovo je neka ideja kako simulirati Propagation.Mandatory u Spring.
    // public async Task<Update.DTO.Response> Update(Update.DTO.Request request)
    // {
    //     var id = request.Id;
    //     var content = request.Content;
    //
    //     var product = await _dbContext.Products.FirstOrDefaultAsync(product => product.Id == id);
    //     if (product == null) throw new Exception("Not found");
    //
    //     product.Description = content.Description;
    //     product.Name = content.Name;
    //
    //     await _logService.Create($"Product {product.Id} has been updated");
    //     await _dbContext.SaveChangesAsync();
    //
    //     return new Update.DTO.Response(product.Id, product.Name, product.Description);
    // }
}