using Microsoft.EntityFrameworkCore.Infrastructure;
using Product.Domain.Common.Logs.Entity;

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
    // Set za proizvode, efektivno je tabela i repozitorijum. Sadrzi sve metode neohpodne za upravljanje.
    public DbSet<Domain.Product.Entity.Product> Products { get; init; }
    
    // Set za logove, efektivno je tabela i repozitorijum. Sadrzi sve metode neohpodne za upravljanje.
    public DbSet<Log> Logs { get; init; }
    
    // Primarno mi sluzi da bi otpocinjao transakije.
    DatabaseFacade Database { get;  }
    // Sluzi mi za perzistenciju promena u bazu podataka. DBContext klasa iz EF ima isti potpis metode.
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}