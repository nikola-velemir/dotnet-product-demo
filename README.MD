# .NET aplikacija za E2

## Predgovor

Ja (vas asistent Nikola) sam svestan da niste radili .NET na nacin gde biste imali pravu Web aplikaciju. Zato sam odlucio da za vreme jednog vikenda napravim mini demo, koji je pokriven komentarima, da steknete osecaj kako to treba otprilike biti odradjeno u .NET.

## O aplikaciji

Aplikacija korsti EF core kao ORM. Pokazuje vam kako se radi injekcija zavisnosti, kao i da imate migracije baze koja vam omogucava laksi kolaborativni rad.

Aplikacija je organizovana u 4 mini projekta, sa petim kao entry point gde se uvezu svi neophodni projekti. Aplikacija prati princi **Clean Architecture**, odnosno podela po slojevima je sledeca:

- **Domain**: Ovaj sloj koristi POCO klase, modeluje entitete iz konkretnog poslovnog domena kojim se aplikacija bavi (u ovom slucaju neki proizvodi). Domen ne bi trebao da ima ikakve zavisnosti (druge vase projekte, i u sustini biblioteke koje skinete preko NuGet), niti da sadrzi logiku orkestracije domenskih entiteta (slucajevi koriscenja), niti nacine perzistencije. Prakticno jako "glup" sloj, sadrzi osnovnu logiku koja modeluje pravila domena. Najcesce sadrzi entitete, servise (klase sa logikom) koji sprovode biznis pravila.

- **Application**: Sloj vrsi okrestraciju domena, odnosno modeluje slucajeve koriscenja. Moze da referencira druge biblioteke (koje skinete kroz NuGet package manager), ali sme da referencira sustinski samo domenske projekte. Ovde cete spoznati servisne klase koje modeluju CRUD operacije. Ovde cete isto videti mnostvo interfejsa (najpre IDbContext), koji sluze kao apstrakcije. Servisne klase znaju samo za ove ugovore, i ne tice ih se kako se konrketna logika sprovodi. Prave implementacije ovih ugovora se nalaze u spoljasnim slojevima (npr. implementacija IDbContext (koji je u ovom sloju) je ProductAppDbContext koji se nalazi u infrastrukturnom sloju). Ovo je DIP princip, to ste radili kod profesora Luburica.

- **Presentation**: Sloj sadrzi REST konrolerske klase, i bavi se prezentacijom sadrzaja i deluje kao prva linija fronta izmedju korisnika i aplikacije. Ovaj sloj sustinski prima zahteve, te te zahteve delegira aplikacionom sloju da se obavi logika slucaja koriscenja, te se vrati rezultat ponovo prezentacionom.

- **Infrastructure**: Primarna uloga sloja je nacin perzistencije podataka. Ovde se nalaze konkretne implementacije repozitorijuma, migacije, kao i konfigurisani nacini kako se domenski entiteti perzistiraju u bazu podataka (IEntityConfiguration implementacije). Pored perzistencije, u slucaju da vam treba komunikacija sa drugim servisom (vasim ili stranim), implementacije ove komunikacije trebaju ovde da se nalaze, a njihovi ugovori (interfejsi) su u aplikacionom sloju.

**Infrastructe** i **Prezentation** slojevi referenciraju **Aplication** sloj.

## Komentar na _Repository pattern_

Radili ste kod profesora Luburica da Repository klase (ili DAO klase) trebaju da imaju svoj interfejs i implementaciju. U .NET to ne zelimo, iz razloga zato sto nam za ralzicite slucajeve koriscenja treba razlicita implementacija.

Za primer:
U jednom slucaju koriscenja dobavljam dodante entitete uz moj entitet. U drom mi treba samo taj osnovni entitet. Ako implementaciju repozitorijuma "zakucate" da mora da dobavi i dodatne entitete, to se tesko menja (postoji nacin koji se zove _Repository Specification pattern_, ali sto se muciti). Umesto toga direktno koristimo DbContext od EF Core, odnosno IDbContext. On je sam po sebi _Repository pattern_. Iako je kontra argument da krsimo DRY princip, ja kazem da je bolje ovde koristiti samo ovaj interfejs svakako (moze se reci da radimo RUG (_Repeat Until Good_) `:)`).

## Komentar na servise u **Aplication** sloju

Ja ne volim da koristim servise za modelovanje slucajeva koriscenja, jer servisi vremenom postanu ogromni. Kad ove klase otvorim, zaboli me glava. Sevise (al bas nesto sto zovemo servisom) koristim kada je neophodna neka specificna biznis logika (kako u domenskom tako i aplikacionom sloju).

Zato vise volim da koristim biblioteku `MediatR`. Ovde imam za jedan konkretan slucaj koriscenja tacno jednu klasu koja modeluje njegovu logiku. Mozete istraziti vise o biblioteci na ovim linkvima:

- [Opste o MediatR](https://mediatr.io/)
- [Primeri koriscenja](https://medium.com/codenx/implement-mediator-pattern-with-mediatr-in-c-8a271d7b9901)

## Komentar na migracije

Migracija baze podataka vam omogucavaju da generisete kod putem `dotnet ef` alata. Migracije mozete primeniti na bazu, kako bi vam se napravila sema baze podataka. Ovo se radi kako bi svi u timu imali isti izgled baze podataka.

Da napravite migaciju uradite sledece:

```bash
dotnet ef migrations add <ime> --project ./Product.Infrastructure --startup-project ./ProductApp
```

ovo ce vam napraviti migraciju sa imenom koje ste zadali. Flag `--project` kaze gde da gleda konfiguracije entiteta, i na kom mestu ce biti fajl za migraciju sacuvan. Flag `--startup-project` kaze koji projekat je onaj od kog se aplikacija podize, ili projekat koji ima intaliran `EntityFrameworkCore.Design` paket.

Da bi ste migaciju primenili, trebate da uradite sledece:

```bash
dotnet ef database update --project ./Product.Infrastructure --startup-project ./ProductApp
```

Ovo ce vam azurirati bazu podataka sa novom migracijom. Drugim recima, generisace vam se tabele i kolone koje niste imali u bazi, a vi ili neko drugi je napravio migraciju.

## Pokretanje aplikacije

Ovde sam vam ostavio `compose.yml`. Putem **Docker-a** (pogledajte sta je), mozete da udarite jednu komandu u terminal, te tako podignete bazu sa konfiguracijom koja je u `compose.yml`. Ovime ne morate isntalirati `PostreSQL` na lokalnu masinu. Ovo mozete koristiti (cak i sta vise savetujem da stalno koristite) na projektu, jer je pristup jako fleksibilan. Prakticno eliminisete potrebu za instalacijom milijardu servisa.

Ako imate **Docker** ili **Docker Desktop** (ovo je za Windows) instaliran, u terminalu samo uradite:

```bash
docker compose up
# ili ako ne zelite da sebi "zabodete" terminal
docker compose up -d
```

Ovo ce povuci **sliku** (nesto kao instalacija ovog servisa) za `PostreSQL`, i pokrenuti je na portu `5432`.

Aplikaciju pokrecete iz **Visual Studio**, ili **Jetbrains Rider**. Aplikacija trci na adresi `http://localhost:5000`. Putem alata **Postman** mozete da joj saljete zahteve.

Kada zavrsite rad sa aplikacijom, u termialu gde ste "podigli" `compose.yml`, uradite sledece:

```bash
docker compose down
```

Ovo ce ugasiti `PostreSQL` servis, i osloboditi port `5432`.

**Sretan rad i sretno istrazivanje `:)`**
