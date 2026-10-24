namespace Product.Application.Product.Features.Get;

/// <summary>
/// DTO objekti sluze kako bi smo vratili samo informacije koje nas interesuju,
/// NE ZELIMO NIKAD da vratimo ceo domenski entitet, te nakon sto iz baze izvucemo domenski entitet,
/// prepakujemo ga u podskup informacija koje zelimo da vratimo frontend aplikaciji.
/// DTO su zapravo ugovori izmedju fronta i backa.
///
/// U ovom primeru koristimo namespace enforcement, iz razloga sto za konrketni slucaj GET,
/// sintaksu u servisnoj klasi mozemo izraziti kao Get.DTO.Request ili Get.DTO.Response.
/// Ovime nema zabune na sta se tacno odnosi dto klasa, odnosno na koji usecase.
/// Ovo je preferenca vaseg asistenta Nikole :).
/// </summary>
public class DTO
{
    public record Request(Guid Id)
    {
        public static Request New(Guid Id) => new(Id);
    }

    public record Response(Guid Id, string Name, string Description, ICollection<Response.Variant> Variants)
    {
        
        public record Variant(
            string Name,
            double Price
        );
    }
}