namespace Product.Domain.Product.Entity;

public class Variant
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public double Price { get; set; }
    
    /// <summary>
    /// Mora jer EF ne moze bez jednog praznog konstruktora
    /// </summary>
    private Variant(){}

    public Variant(Guid id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
    public static Variant Create(string name, double price)=>
        new Variant(Guid.NewGuid(), name, price);
}