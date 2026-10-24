namespace Product.Domain.Product.Entity;

public class Product
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Description { get; set; }
    private readonly ICollection<Variant> _variants = new List<Variant>();
    public ICollection<Variant> Variants => _variants;

    public Product(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    /// <summary>
    /// Mora jer EF ne moze bez jednog praznog konstruktora
    /// </summary>
    private Product(){}
    
    public static Product Create(string name, string description)
    => new Product(Guid.NewGuid(), name, description);
}