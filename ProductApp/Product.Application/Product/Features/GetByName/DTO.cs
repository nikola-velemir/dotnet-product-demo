namespace Product.Application.Product.Features.GetByName;

public class DTO
{
    public record Request(string Name);
    public record Response(Guid Id, string Name, string Description);
}