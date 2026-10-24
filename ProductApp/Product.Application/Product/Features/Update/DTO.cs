namespace Product.Application.Product.Features.Update;

public class DTO
{
    public record Request(Guid Id, Request.Body Content)
    {
        public record Body(string Name, string Description);
    }

    public record Response(Guid Id, string Name, string Description);
}