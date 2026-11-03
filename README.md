# portfolio api

> Deze api heb ik zelf gebouwd. Claude heeft me geholpen met uitleg en voorbeelden en met het duidelijker maken van deze readme.

De backend van mijn portfoliosite, gemaakt met ASP.NET Core (.NET 10), EF Core, Identity en SQL Server. Iedereen kan projecten en blogposts bekijken. Alleen een Editor of Admin kan ze aanmaken, wijzigen en verwijderen.

## wat je nodig hebt

- .NET 10 SDK
- SQL Server op localhost
- de tool voor migraties, die installeer je een keer met:

```bash
dotnet tool install --global dotnet-ef
```

De packages haalt `dotnet run` zelf op. De connection string staat in `appsettings.json`, daar staat geen wachtwoord in omdat je inlogt met je Windows account.

## Starten

Let op dat je in de goede map zit. Je moet twee keer de projectnaam zien, dus `EersteWebNetApplicatie\EersteWebNetApplicatie`. Zie je hem maar een keer, doe dan eerst:

```bash
cd EersteWebNetApplicatie
```

Dan de database maken of bijwerken:

```bash
dotnet ef database update
```

En de api starten:

```bash
dotnet run
```

## Swagger gebruiken

Swagger is de pagina waarop je de api kan testen. Die staat op http://localhost:5029/swagger. Start je met het https profiel in Visual Studio, dan is het https://localhost:7241/swagger.

Zo stuur je een request:

1. Klik op een route, bijvoorbeeld `POST /Account/register`.
2. Klik op **Try it out**.
3. Vul de JSON in.
4. Klik op **Execute**. Onder **Server response** zie je het antwoord van de api.

Log je in via `POST /Account/login`, dan bewaart je browser een cookie en stuurt die daarna zelf mee. Je blijft dus ingelogd voor de volgende requests.

## Migraties

Een migratie is een script dat de tabellen in je database maakt of aanpast. De scripts staan in de map `Migrations`. `dotnet ef database update` voert alle scripts uit die nog niet zijn uitgevoerd, dat noem je een migratie toepassen. Op een nieuwe pc maakt dit de hele database `PortfolioDb` aan, dus ook de Identity tabellen zoals `AspNetUsers` en `AspNetRoles`.

Verander je een model, dan maak je eerst een nieuwe migratie en voer je die daarna uit:

```bash
dotnet ef migrations add NaamVanDeWijziging
```

```bash
dotnet ef database update
```

Ik gebruik migraties en geen EnsureCreated, omdat ik dan later een veld kan toevoegen zonder de hele database weg te gooien.

## Gebruikers en rollen

De rollen Gast, Editor en Admin worden bij het opstarten aangemaakt als ze nog niet bestaan.

Een gewone gebruiker maak je in Swagger met `POST /Account/register`. Die krijgt altijd de rol Gast, via de api kan je jezelf dus geen Editor of Admin maken.

Wil je iemand Editor of Admin maken, open dan SQL Server en voer deze query uit. Verander de email en de rol:

```sql
USE PortfolioDb;

INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.UserName = 'user@example.com' AND r.Name = 'Editor';
```

Weet je de email niet meer, dan zoek je op een stukje ervan. Zo hoef je niet door alle users heen te kijken:

```sql
SELECT Id, UserName FROM AspNetUsers WHERE UserName LIKE '%stukje%';
```

De namen van de rollen zie je met:

```sql
SELECT Name FROM AspNetRoles;
```

Log daarna opnieuw in, want je rollen staan in je cookie en die verandert niet vanzelf.

## Endpoints

| methode | route               | wie mag dit     |
| ------- | ------------------- | --------------- |
| POST    | `/Account/register` | iedereen        |
| POST    | `/Account/login`    | iedereen        |
| GET     | `/Projects`         | iedereen        |
| GET     | `/Projects/{id}`    | iedereen        |
| POST    | `/Projects`         | Editor of Admin |
| PUT     | `/Projects/{id}`    | Editor of Admin |
| DELETE  | `/Projects/{id}`    | Editor of Admin |
| GET     | `/BlogPost`         | iedereen        |
| GET     | `/BlogPost/{id}`    | iedereen        |
| POST    | `/BlogPost`         | Editor of Admin |
| PUT     | `/BlogPost/{id}`    | Editor of Admin |
| DELETE  | `/BlogPost/{id}`    | Editor of Admin |

Niet ingelogd geeft 401. Wel ingelogd maar zonder de goede rol geeft 403.

## Tests draaien

De tests staan in het project `EersteWebNetApplicatie.Tests`. Ze sturen echte HTTP requests naar de api, net als Swagger. Ze testen alle routes uit de tabel hierboven:

- zonder login, als Gast, als Editor en als Admin
- met ongeldige invoer en met een id dat niet bestaat
- of de response de goede statuscode en data heeft
- of een POST, PUT of DELETE echt iets in de database verandert

De tests gebruiken een aparte database, `PortfolioDb_Test`, zodat je echte data niet wordt aangeraakt. Welke database dat is staat in `appsettings.Testing.json`.

Maak die database een keer aan. Dit voert de migraties uit op de testdatabase, net als `dotnet ef database update`, maar dan op `PortfolioDb_Test` in plaats van op `PortfolioDb`. Doe dit in de map `EersteWebNetApplicatie\EersteWebNetApplicatie`:

```bash
dotnet ef database update --connection "Server=localhost;Database=PortfolioDb_Test;Trusted_Connection=True;TrustServerCertificate=True"
```

Na een nieuwe migratie doe je dit opnieuw.

Draai daarna de tests vanuit de bovenste map, waar het `.slnx` bestand staat:

```bash
dotnet test
```

De rollen maakt de api zelf aan als de tests hem starten.

## Hoe het in elkaar zit

Een request gaat zo door de api: controller → service → `PortfolioDbContext` → SQL Server.

| onderdeel            | wat doet het                                                               |
| -------------------- | -------------------------------------------------------------------------- |
| `Controllers`        | de routes en statuscodes, `AccountController` doet registreren en inloggen |
| `Services`           | de logica en de database, weet niks van HTTP                               |
| `Models`             | `Project` en `Blogpost`, mijn tabellen                                     |
| `DTOs`               | wat de api binnenkrijgt en terugstuurt                                     |
| `PortfolioDbContext` | de verbinding met de database, ook voor de Identity tabellen               |

## Mijn keuzes

**Controllers en geen Minimal API:** Daar zijn we in les 5 mee begonnen en zo begrijp ik het het beste. Alles van projecten staat bij elkaar in `ProjectsController` en alles van blogposts in `BlogPostController`.

**Lagen:** Gaat er iets mis met een statuscode dan kijk ik in de controller, gaat er iets mis met de data dan kijk ik in de service. Voor twee onderdelen is dit genoeg.

**Geen repository en geen interfaces:** Toen ik begon wist ik nog niet wat een repository was. Ik snap nu wat het voordeel is, maar voor deze opdracht vind ik het onnodig om het nog toe te voegen. Hetzelfde geldt voor interfaces, omdat mijn tests via HTTP gaan.

**DTO's:** De api stuurt nooit direct een model terug. Voeg ik later een veld toe dat niemand mag zien, dan gaat dat zo niet per ongeluk mee naar buiten. De request heeft geen ID omdat de database die maakt, de response wel.

**Dependency injection:** De services en de DbContext staan in `Program.cs`. Ik krijg ze via de constructor en maak ze nergens zelf aan met `new`.

**Cookies:** Na het inloggen krijg je een cookie en die stuurt de browser daarna zelf mee. Zo weet de api bij elk request wie je bent. Identity, het loginsysteem van ASP.NET Core, slaat wachtwoorden gehasht op.

**AI:** Ik heb Claude gebruikt om deze readme zo mooi op te zetten
