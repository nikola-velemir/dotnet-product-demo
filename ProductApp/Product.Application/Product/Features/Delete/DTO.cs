namespace Product.Application.Product.Features.Delete;

public class DTO
{
    public record Request(Guid id);
    public record Response(Guid id);
}