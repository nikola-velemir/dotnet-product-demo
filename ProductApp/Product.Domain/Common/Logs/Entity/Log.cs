namespace Product.Domain.Common.Logs.Entity;

public class Log
{
    public Guid Id { get; init; }
    public string Action { get; init; }
    public DateTime Timestamp { get; init; }

    //Mora jer se EF zbuni
    private Log()
    {
    }

    // kontstruktor sa svim parametrima
    public Log(Guid id, string action, DateTime timestamp)
    {
        Id = id;
        Action = action;
        Timestamp = timestamp;
    }

    // factory metoda
    public static Log Create(string action)
        => new Log(Guid.NewGuid(), action, DateTime.UtcNow);
}