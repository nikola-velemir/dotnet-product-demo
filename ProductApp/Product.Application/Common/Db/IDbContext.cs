namespace Product.Application.Common.Db;

using Microsoft.EntityFrameworkCore;

/// <summary>
/// Apstrakcija koja je repository pattern. U .NET, nasuprot Spring Boot, pozeljno je samo koristiti
/// DbContext i njega injekotvati u servisne klase. Ovo se radi iz razloga sto kada repository napravite jednom,
/// tesko je menjati njeno interno funkcionisanje.
/// Putem DbContext mozete tacno specificirati query u bazu za vas usecase.
/// </summary>
public interface IDbContext
{
    public DbSet<Domain.Product.Entity.Product> Products { get; init; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}