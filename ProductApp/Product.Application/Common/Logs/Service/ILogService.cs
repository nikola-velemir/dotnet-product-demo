namespace Product.Application.Common.Logs.Service;

public interface ILogService
{
    Task Create(string action);
}