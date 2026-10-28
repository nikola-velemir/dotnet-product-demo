# .NET aplikacija za E2

## **OVAJ PROJEKAT JE DEMONSTRATIVNOG KARAKTERA**

## Predgovor

Ja (vaš asistent Nikola) svestan sam da niste radili .NET na način na koji biste imali pravu web aplikaciju. Zato sam odlučio da tokom jednog vikenda napravim mini demo, pokriven komentarima, kako biste stekli osećaj kako bi to otprilike izgledalo u .NET-u.


## O aplikaciji

Aplikacija koristi EF Core kao ORM. Pokazuje vam kako se radi injekcija zavisnosti, a imate i migracije baze koje vam omogućavaju lakši kolaborativni rad.

Aplikacija je organizovana u 4 mini projekta, sa petim kao ulaznom tačkom (entry point) u kojoj se povežu svi neophodni projekti. Aplikacija prati princip **Clean Architecture**, odnosno podela po slojevima je sledeća:

- **Domain**: Ovaj sloj koristi POCO klase i modeluje entitete iz konkretnog poslovnog domena kojim se aplikacija bavi (u ovom slučaju neki proizvodi). Domen ne bi trebalo da ima nikakve zavisnosti (druge vaše projekte, a u suštini ni biblioteke koje skinete preko NuGet-a), niti da sadrži logiku orkestracije domenskih entiteta (slučajeve korišćenja), niti načine perzistencije. Praktično, veoma „glup" sloj koji sadrži osnovnu logiku koja modeluje pravila domena. Najčešće sadrži entitete i servise (klase sa logikom) koji sprovode poslovna pravila.

- **Application**: Sloj vrši orkestraciju domena, odnosno modeluje slučajeve korišćenja. Može da referencira druge biblioteke (koje skinete kroz NuGet package manager), ali sme da referencira suštinski samo domenske projekte. Ovde ćete upoznati servisne klase koje modeluju CRUD operacije. Ovde ćete takođe videti mnoštvo interfejsa (pre svega IDbContext), koji služe kao apstrakcije. Servisne klase znaju samo za ove ugovore i ne tiče ih se kako se konkretna logika sprovodi. Prave implementacije ovih ugovora nalaze se u spoljašnjim slojevima (npr. implementacija IDbContext-a (koji je u ovom sloju) je ProductAppDbContext, koji se nalazi u infrastrukturnom sloju). Ovo je DIP princip, to ste radili kod profesora Luburića.

- **Presentation**: Sloj sadrži REST kontrolerske klase i bavi se prezentacijom sadržaja, te deluje kao prva linija fronta između korisnika i aplikacije. Ovaj sloj suštinski prima zahteve i delegira ih aplikacionom sloju, koji obavlja logiku slučaja korišćenja, nakon čega se rezultat vraća prezentacionom sloju.

- **Infrastructure**: Primarna uloga sloja je način perzistencije podataka. Ovde se nalaze konkretne implementacije repozitorijuma, migracije, kao i konfigurisani načini na koje se domenski entiteti perzistiraju u bazu podataka (IEntityConfiguration implementacije). Pored perzistencije, ako vam treba komunikacija sa drugim servisom (vašim ili tuđim), implementacije te komunikacije treba da se nalaze ovde, a njihovi ugovori (interfejsi) u aplikacionom sloju.

**Infrastructure** i **Presentation** slojevi referenciraju **Application** sloj.

## Komentar na _Repository pattern_

Radili ste kod profesora Luburića da Repository klase (ili DAO klase) treba da imaju svoj interfejs i implementaciju. U .NET-u to ne želimo, zato što nam za različite slučajeve korišćenja treba različita implementacija.

Na primer: u jednom slučaju korišćenja dobavljam dodatne entitete uz moj entitet, a u drugom mi treba samo taj osnovni entitet. Ako implementaciju repozitorijuma „zakucate" tako da mora da dobavi i dodatne entitete, to se teško menja (postoji način koji se zove _Repository Specification pattern_, ali što se mučiti). Umesto toga direktno koristimo DbContext iz EF Core-a, odnosno IDbContext. On je sam po sebi _Repository pattern_. Iako je kontraargument da kršimo DRY princip, ja kažem da je bolje u svakom slučaju ovde koristiti samo ovaj interfejs (može se reći da radimo RUG (_Repeat Until Good_)) `:)`.

## Komentar na servise u **Application** sloju

Ja ne volim da koristim servise za modelovanje slučajeva korišćenja, jer servisi vremenom postanu ogromni. Kad ove klase otvorim, zaboli me glava. Servise (ali baš nešto što zovemo servisom) koristim kada je neophodna neka specifična poslovna logika (kako u domenskom, tako i u aplikacionom sloju).

Zato više volim da koristim biblioteku `MediatR`. Tu imam za jedan konkretan slučaj korišćenja tačno jednu klasu koja modeluje njegovu logiku. Više o biblioteci možete istražiti na ovim linkovima:

- [Opšte o MediatR-u](https://mediatr.io/)
- [Primeri korišćenja](https://medium.com/codenx/implement-mediator-pattern-with-mediatr-in-c-8a271d7b9901)

## Komentar na migracije

Migracije baze podataka omogućavaju vam da generišete kod za migraciju putem alata `dotnet ef`. Migracije možete primeniti na bazu kako bi vam se napravila šema baze podataka. Ovo se radi kako bi svi u timu imali isti izgled baze podataka.

Da biste napravili migraciju, uradite sledeće:

```bash
dotnet ef migrations add <ime> --project ./Product.Infrastructure --startup-project ./ProductApp
```

Ovo će vam napraviti migraciju sa imenom koje ste zadali. Flag `--project` kaže gde da gleda konfiguracije entiteta i na kom mestu će fajl za migraciju biti sačuvan. Flag `--startup-project` kaže koji projekat je onaj iz kog se aplikacija podiže, odnosno projekat koji ima instaliran paket `EntityFrameworkCore.Design`.

Da biste migraciju primenili, treba da uradite sledeće:

```bash
dotnet ef database update --project ./Product.Infrastructure --startup-project ./ProductApp
```

Ovo će ažurirati bazu podataka novom migracijom. Drugim rečima, generisaće se tabele i kolone koje niste imali u bazi, a koje je neko napravio kroz migraciju.

## Pokretanje aplikacije

Ovde sam vam ostavio `compose.yml`. Putem **Docker-a** (pogledajte šta je) možete da unesete jednu komandu u terminal i tako podignete bazu sa konfiguracijom koja je u `compose.yml`. Time ne morate da instalirate `PostgreSQL` na lokalnu mašinu. Ovo možete koristiti (štaviše, savetujem da stalno koristite) na projektu, jer je pristup veoma fleksibilan. Praktično eliminišete potrebu za instalacijom milijardu servisa.

Ako imate instaliran **Docker** ili **Docker Desktop** (ovo je za Windows), u terminalu samo uradite:

```bash
docker compose up
# ili ako ne želite da vam zauzme terminal
docker compose up -d
```

Ovo će povući **sliku** (nešto kao instalacija ovog servisa) za `PostgreSQL` i pokrenuti je na portu `5432`.

Aplikaciju pokrećete iz **Visual Studija** ili **JetBrains Ridera**. Aplikacija radi na adresi `http://localhost:5000`. Putem alata **Postman** možete da joj šaljete zahteve.

Kada završite rad sa aplikacijom, u terminalu gde ste pokrenuli `compose.yml` uradite sledeće:

```bash
docker compose down
```

Ovo će ugasiti `PostgreSQL` servis i osloboditi port `5432`.

**Srećan rad i uspešno istraživanje** `:)`
