namespace Product.Application.Product.Features.Create;

public class DTO
{
    public record Request(string Name, string Description);

    public record Response(Guid Id, string Name, string Description);
}