using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Product.Application.Common.Db;
using Product.Domain.Common.Logs.Entity;

namespace Product.Application.Common.Logs.Service;

/// <summary>
/// Opet internal da ne bi drugi asembliji pristupali implementaciji
/// </summary>
internal class LogService : ILogService
{
    private readonly IDbContext _dbContext;

    public LogService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    private void EnsureTransactionExists()
    {
        // ovde se vrsi provera da li postoji vec otpoceta transakcija, kako bi joj se prikljucio
        // Ovo u sustini simulira Propagation.Mandatory u Spring, unutar anotacije @Transactional
        // Pod prikljucivanjem smatram da je implicitno, deli isti DbContext koji je scoped.
        var hasEfTransaction = _dbContext.Database.CurrentTransaction is not null;
        var hasAmbientTransaction = Transaction.Current is not null;

        if (!hasEfTransaction && !hasAmbientTransaction)
        {
            throw new InvalidOperationException(
                "No active transaction. LogService.Create requires an existing transaction (MANDATORY).");
        }
    }
    public async Task Create(string action)
    {
        EnsureTransactionExists();
        var log = Log.Create(action);
        _dbContext.Logs.Add(log);
        await _dbContext.SaveChangesAsync();
    }
}