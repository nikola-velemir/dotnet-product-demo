namespace Product.Domain.Product.Entity;

public class Product
{
    // kazem da je setter init, ne zelim da ikada menjam ID
    public Guid Id { get; init; }
    // Ovde moze setter da bude set;, zelim da menjam ime prilikom update.
    // Ovo se moglo lezernije odradit da je private set;, pa imamo metodu koja odradi update.
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
    
    /// <summary>
    ///  Factory metoda da kreira novi proizvod
    /// </summary>
    /// <param name="name">Ime proizvoda</param>
    /// <param name="description">Opis proizvoda</param>
    /// <returns>Novi entitet product</returns>
    public static Product Create(string name, string description)
    => new Product(Guid.NewGuid(), name, description);
}