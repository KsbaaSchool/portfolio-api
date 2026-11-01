# portfolio api

> deze api heb ik zelf gebouwd. claude heeft me geholpen met uitleg en voorbeelden en met het duidelijker maken van deze readme.

de backend van mijn portfoliosite, gemaakt met ASP.NET Core (.NET 10), EF Core, Identity en SQL Server. iedereen kan projecten en blogposts bekijken. alleen een Editor of Admin kan ze aanmaken, wijzigen en verwijderen.

## wat je nodig hebt

- .NET 10 SDK
- SQL Server op localhost
- de tool voor migraties, die installeer je een keer met:

```bash
dotnet tool install --global dotnet-ef
```

de packages haalt `dotnet run` zelf op. de connection string staat in `appsettings.json`, daar staat geen wachtwoord in omdat je inlogt met je windows account.

## starten

let op dat je in de goede map zit. je moet twee keer de projectnaam zien, dus `EersteWebNetApplicatie\EersteWebNetApplicatie`. zie je hem maar een keer, doe dan eerst:

```bash
cd EersteWebNetApplicatie
```

dan de database maken of bijwerken:

```bash
dotnet ef database update
```

en de api starten:

```bash
dotnet run
```

swagger staat daarna op http://localhost:5029/swagger. start je met het https profiel in visual studio, dan is het https://localhost:7241/swagger.

## migraties

een migratie is een script dat de tabellen in je database maakt of aanpast. de scripts staan in de map `Migrations`. `dotnet ef database update` voert alle scripts uit die nog niet zijn uitgevoerd, dat noem je een migratie toepassen. op een nieuwe pc maakt dit de hele database `PortfolioDb` aan, dus ook de Identity tabellen zoals `AspNetUsers` en `AspNetRoles`.

verander je een model, dan maak je eerst een nieuwe migratie en voer je die daarna uit:

```bash
dotnet ef migrations add NaamVanDeWijziging
```

```bash
dotnet ef database update
```

ik gebruik migraties en geen EnsureCreated, omdat ik dan later een veld kan toevoegen zonder de hele database weg te gooien.

## gebruikers en rollen

de rollen Gast, Editor en Admin worden bij het opstarten aangemaakt als ze nog niet bestaan.

een gewone gebruiker maak je met `POST /Account/register`, die krijgt altijd de rol Gast. via de api kan je jezelf dus geen Editor of Admin maken.

wil je iemand Editor of Admin maken, doe dit dan in SQL Server. verander de email en de rol:

```sql
USE PortfolioDb;

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.UserName = 'user@example.com' AND r.Name = 'Editor';
```

weet je de email niet meer, dan zoek je op een stukje ervan. zo hoef je niet door alle users heen te kijken:

```sql
SELECT Id, UserName FROM AspNetUsers WHERE UserName LIKE '%stukje%';
```

de namen van de rollen zie je met:

```sql
SELECT Name FROM AspNetRoles;
```

log daarna opnieuw in, want je rollen staan in je cookie en die verandert niet vanzelf.

## endpoints

| methode | route | wie mag dit |
| --- | --- | --- |
| POST | `/Account/register` | iedereen |
| POST | `/Account/login` | iedereen |
| GET | `/Projects` | iedereen |
| GET | `/Projects/{id}` | iedereen |
| POST | `/Projects` | Editor of Admin |
| PUT | `/Projects/{id}` | Editor of Admin |
| DELETE | `/Projects/{id}` | Editor of Admin |
| GET | `/BlogPost` | iedereen |
| GET | `/BlogPost/{id}` | iedereen |
| POST | `/BlogPost` | Editor of Admin |
| PUT | `/BlogPost/{id}` | Editor of Admin |
| DELETE | `/BlogPost/{id}` | Editor of Admin |

niet ingelogd geeft 401. wel ingelogd maar zonder de goede rol geeft 403.

## hoe het in elkaar zit

een request gaat zo door de api: controller → service → `PortfolioDbContext` → SQL Server.

| onderdeel | wat doet het |
| --- | --- |
| `Controllers` | de routes en statuscodes. `AccountController` doet registreren en inloggen |
| `Services` | de logica en de database. weet niks van HTTP |
| `Models` | `Project` en `Blogpost`, mijn tabellen |
| `DTOs` | wat de api binnenkrijgt en terugstuurt |
| `PortfolioDbContext` | de verbinding met de database, ook voor de Identity tabellen |

## mijn keuzes

**controllers en geen Minimal API:** daar zijn we in les 5 mee begonnen en zo begrijp ik het het beste. alles van projecten staat bij elkaar in `ProjectsController` en alles van blogposts in `BlogPostController`.

**lagen:** gaat er iets mis met een statuscode dan kijk ik in de controller, gaat er iets mis met de data dan kijk ik in de service. voor twee onderdelen is dit genoeg.

**DTO's:** de api stuurt nooit direct een model terug. voeg ik later een veld toe dat niemand mag zien, dan gaat dat zo niet per ongeluk mee naar buiten. de request heeft geen ID omdat de database die maakt, de response wel.

**dependency injection:** de services en de DbContext staan in `Program.cs`. ik krijg ze via de constructor en maak ze nergens zelf aan met `new`.

**cookies:** na het inloggen krijg je een cookie en die stuurt de browser daarna zelf mee. zo weet de api bij elk request wie je bent. wachtwoorden worden door Identity gehasht opgeslagen.
