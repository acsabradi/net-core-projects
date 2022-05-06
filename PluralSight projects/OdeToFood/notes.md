- [1. Alapok](#1-alapok)
  - [1.1. Új Razor page link felvétele a főoldalon](#11-új-razor-page-link-felvétele-a-főoldalon)
  - [1.2. Új Razor page létrehozása](#12-új-razor-page-létrehozása)
  - [1.3. Konfiguráció injektálása és felhasználása](#13-konfiguráció-injektálása-és-felhasználása)
  - [1.4. Entity és data service létrehozása](#14-entity-és-data-service-létrehozása)
  - [1.5. A data service felhasználása a modelben](#15-a-data-service-felhasználása-a-modelben)
  - [1.6. Az étterem lista kiíratása](#16-az-étterem-lista-kiíratása)
- [2. Model binding](#2-model-binding)
  - [2.1. Form létrehozása](#21-form-létrehozása)
  - [2.2. A data service módosítása](#22-a-data-service-módosítása)
  - [2.3. Bind létrehozása a model és a form között](#23-bind-létrehozása-a-model-és-a-form-között)
  - [2.4. Search string lementése](#24-search-string-lementése)
  - [2.5. Detail oldal](#25-detail-oldal)
    - [2.5.1. Detail model és UI](#251-detail-model-és-ui)
    - [2.5.2. Étterem ID átadása query string-el](#252-étterem-id-átadása-query-string-el)
    - [2.5.3. Path alapú paraméterátadás query string helyett](#253-path-alapú-paraméterátadás-query-string-helyett)
    - [2.5.4. Lekérdezés ID alapján](#254-lekérdezés-id-alapján)
    - [2.5.5. Hibakezelés](#255-hibakezelés)
- [3. Rekordok szerkesztése](#3-rekordok-szerkesztése)
  - [3.1. *Edit* page létrehozása](#31-edit-page-létrehozása)
  - [3.2. *Edit* page form](#32-edit-page-form)
    - [3.2.1. Enum kezelése a model-ben](#321-enum-kezelése-a-model-ben)
  - [3.3. POST bind](#33-post-bind)
  - [3.4. Az `enum` probléma kezelése](#34-az-enum-probléma-kezelése)
  - [3.5. Form input validálása](#35-form-input-validálása)
  - [3.6. Post-Redirect-Get (PRG) minta](#36-post-redirect-get-prg-minta)
  - [3.7. Új étterem rekord létrehozása](#37-új-étterem-rekord-létrehozása)
    - [3.7.1. Az *Edit* újrafelhasználása](#371-az-edit-újrafelhasználása)
    - [3.7.2. Data service frissítése](#372-data-service-frissítése)
    - [3.7.3. Új étterem lekezelése az *Edit* `OnPost`-ban](#373-új-étterem-lekezelése-az-edit-onpost-ban)
  - [3.8. Visszajelzés a sikeres módosításról](#38-visszajelzés-a-sikeres-módosításról)
- [4. SQL Server és Entity Framework Core](#4-sql-server-és-entity-framework-core)
  - [4.1. NuGet függőségek](#41-nuget-függőségek)
  - [4.2. Leszármazás `DbContext`-ből](#42-leszármazás-dbcontext-ből)
  - [4.3. dotnet Entity Framework Core (EF) CLI](#43-dotnet-entity-framework-core-ef-cli)
  - [4.4. Adatbázis service konfigurálása](#44-adatbázis-service-konfigurálása)
  - [4.5. Adatbázis migráció](#45-adatbázis-migráció)
  - [4.6. SQL Server data access service implementálása](#46-sql-server-data-access-service-implementálása)
- [5. UI fejlesztése](#5-ui-fejlesztése)
  - [5.1. A *_Layout.cshtml* működése](#51-a-_layoutcshtml-működése)
  - [5.2. *Delete* page a default layout nélkül](#52-delete-page-a-default-layout-nélkül)
  - [5.3. *_ViewStart* és *_ViewImports*](#53-_viewstart-és-_viewimports)
  - [5.4. Partial View](#54-partial-view)
  - [5.5. View Component](#55-view-component)
  - [5.6. Partial View és View Component megkülönböztetése](#56-partial-view-és-view-component-megkülönböztetése)
  - [5.7. Scaffolding](#57-scaffolding)
- [6. Kliens oldali JavaScript és CSS kód integrálása](#6-kliens-oldali-javascript-és-css-kód-integrálása)
  - [6.1. Statikus fájlok kezelése](#61-statikus-fájlok-kezelése)
  - [6.2. Kliens oldali validálás](#62-kliens-oldali-validálás)
  - [API controller használata](#api-controller-használata)

# 1. Alapok

## 1.1. Új Razor page link felvétele a főoldalon

Az oldalak struktúráját megadó *.cshtml* fájlok a *Pages* mappában találhatók. Itt megnyitjuk a *\Shared\\_Layout.cshtml* fájlt, mert ez tartalmazza a minden oldalra alkalmazandó elemeket (header, navbar, footer).

Megkeressük a navbar-t és kiegészítjük egy olyan linkkel, ami az étterem listát hozza fel:

```html
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-page="/Restaurants/List">Restaurants</a>
</li>
```

Az `asp-page` attribútum megadja az étterem listát megadó fájl path-ját. Minden ASP.NET-hez köthető (tehát nem szabvány HTML5) attribútum `asp`-vel kezdődik.

## 1.2. Új Razor page létrehozása

Létrehozunk egy *Restaurants* mappát a *Pages*-ben, majd ott egy üres Razor page-t, amit *List*-nek nevezünk el. Így létrehoztuk azt a komponenst, amit az előbbi `asp-page` attribútum céloz meg.

Az új komponens 2 fájlból áll:
- *List.cshtml*: Az oldal felépítését adja meg HTML struktúrában, ezenfelül C# kódot is képes futtatni.
- *List.cshtml.cs*: Az oldal mögötti logikát realizáló osztályt tartalmazza (model), amit a dotnet automatikusan *ListModel*-nek nevez el.

## 1.3. Konfiguráció injektálása és felhasználása

Az *appsettings.json* fájlban tetszőleges szöveges adatot tárolhatunk, beleteszünk egy string-et:

```json
"Message": "Hello from settings file"
```

Azt akarjuk elérni, hogy a Razor page felhasználja ezt a settings fájlt, valamint kiírja annak egy elemét az oldalra.

Az ASP.NET a Dependency Injection elvet alkalmazza a függőségek kezelésére, így a komponensek interfészeken keresztül kapják meg a kontrollt a service-ek felett. A settings tartalma is egy service.

A `ListModel` konstruktorát úgy módosítjuk, hogy paraméterként kapjon egy `IConfiguration` interfészt és azt mentse le egy field-be:

```cs
private readonly IConfiguration config;

public ListModel(IConfiguration config)
{
    this.config = config;
}
```

Magát a kiírandó üzenetet nem a konstruktorban, hanem az `OnGet` metódusban mentjük el:

```cs
public string Message { get; set; }

public void OnGet()
{
    Message = config["Message"];
}
```

Ez a függvény minden GET parancsnál (pl. az oldal betöltése esetén) le fog futni. A settings fájlból kiolvassa a `Message`-hez rendelt értéket és lementi egy property-ba.

A *List.cshtml*-t módosítjuk, hogy megjelenjen ez a property:

```html
<div>@Model.Message</div>
```

A `@` karakterrel jelöljük, ha C# kódot akarunk használni. A `Model`-el hivatkozunk a page model osztályára. Mivel az oldal megjelenítése lőtt már lefutott a model konstruktora és az `OnGet`, így a `Message` már inicializálva lesz.

## 1.4. Entity és data service létrehozása

Egy külön projektben létrehozunk egy `Restaurant` osztályt és benne property-ket.

Egy másik projektben kezeljük a data service-t. Ezt is interfészként fogjuk injektálni a `ListModel`-be, ezért először kell egy interfész:

```cs
public interface IRestaurantData
{
    IEnumerable<Restaurant> GetAll();
}
```

Ez a service egy memóriában tárolt listából fog adatot szolgáltatni, a konstruktora ezt inicializálja:

```cs
public class InMemoryRestaurantData : IRestaurantData
{
    List<Restaurant> restaurants;

    public InMemoryRestaurantData()
    {
        restaurants = new List<Restaurant>()
        {
            new Restaurant { Id = 1, Name = "Scott's Pizza", Location = "Maryland", Cuisine = CuisineType.Italian },
            new Restaurant { Id = 2, Name = "Cinnamon Club", Location = "London", Cuisine = CuisineType.Italian },
            new Restaurant { Id = 3, Name = "La Costa", Location = "California", Cuisine = CuisineType.Mexican },
        };
    }
    //...
}
```

Implementáljuk az interfészt. A függvény egy LINQ parancssal visszaad minden elemet a listából, név szerint sorbarendezve:

```cs
public class InMemoryRestaurantData : IRestaurantData
{
    //...
    public IEnumerable<Restaurant> GetAll()
    {
        return from r in restaurants
                orderby r.Name
                select r;
    }
}
```

Regisztrálni kell az elkészült service-t, ezt a *Startup.cs*-ben tehetjük meg:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IRestaurantData, InMemoryRestaurantData>();
    //...
}
```

Singleton service-ként regisztáltuk, mert csak egy példányt kérünk ebből a service-ből.

## 1.5. A data service felhasználása a modelben

Módosítjuk a model konstruktort úgy, hogy az új service interfészt is fogadja. Ezt a service-t is lementjük egy field-be:

```cs
private readonly IRestaurantData restaurantData;

public ListModel(IConfiguration config, IRestaurantData restaurantData)
{
    this.config = config;
    this.restaurantData = restaurantData;
}
```

Az `OnGet`-nek le kell mentenie a service által rendelkezésre bocsátott étterem listát:

```cs
public IEnumerable<Restaurant> Restaurants { get; set; }

public void OnGet(string searchTerm)
{
    Message = config["Message"];
    Restaurants = restaurantData.GetAll();
}
```

## 1.6. Az étterem lista kiíratása

A *List.cshtml*-ben létrehozunk egy táblázatot, majd a modelben tárolt listát iterálva feltöltjük azt:

```html
<table class="table">
    @foreach(var restaurant in Model.Restaurants)
    {
    <tr>
        <td>@restaurant.Name</td>
        <td>@restaurant.Location</td>
        <td>@restaurant.Cuisine</td>
    </tr>
    }
</table>
```

# 2. Model binding

## 2.1. Form létrehozása

Létrehozunk a *List.cshtml*-ben egy szöveges bemenetet váró mezőből és egy ikonos gombból álló form-ot:

```html
<form method="get">
    <div class="input-group mb-3">
        <input type="search" class="form-control" value="" name="searchTerm" />
        <button class="btn btn-outline-secondary">
            <i class="bi bi-search"></i>
        </button>
    </div>
</form>
```

A form által megadott szöveggel majd keresni lehet az éttermek között:
- A form GET metódust fog triggerelni, mivel adatlekérést végez el. Másik előnye (pl. a POST-hoz képest), hogy a megadott string így belekerül az URl query string-jébe, így a keresés eredménye könyvjelzőzhető.
- `input type="search"`: Olyan string-et adunk meg, ami alapján keresést indítunk.
- `name="searchTerm"`: A model a `searchTerm` változónévvel tud majd referálni a form-ban megadott értékre.
- A `class` attribútumoknál Bootstrap osztályokat használunk a megjelenés formázására. A tréninganyagban szereplő forráskód már elavult, a [Bootstrap form dokumentációja](https://getbootstrap.com/docs/5.1/forms/input-group/#button-addons) írja le a helyes beállítást.
- Az ikon beszúrás szintén elavult. A [Bootstrap Icon CDN-jét](https://icons.getbootstrap.com/#cdn) be kell illeszteni a *_Layout.cshtml* head szekciójába, mert alapértelmezetten az ikonok nem részei a Bootstrap-nek. Ezután már használható az ikonok osztályai.

## 2.2. A data service módosítása

Módosítanunk kell a data service-t úgy, hogy tudja kezelni a keresést. Először az interfészt írjuk át, hogy a metódus fogadjon egy string paramétert, ami alapján elvégezhető a keresés:

```cs
public interface IRestaurantData
{
    IEnumerable<Restaurant> GetRestaurantsByName(string name);
}
```

A módosított metódus:

```cs
public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
{
    return from r in restaurants
            where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
            orderby r.Name
            select r;
}
```

A LINQ query bővült egy `where` kifejezéssel, ez felel a keresésért. A `where` kiértékelése `true` lesz ha az étteremnév a megadott string-el kezdődik (a `StartsWith` case-sensitive) vagy a megadott string üres vagy `null`. Tehát a query minden rekordot vissza fog adni üres string vagy `null` bemenet esetén.

## 2.3. Bind létrehozása a model és a form között

A modelben frissítenünk kell az `OnGet` metódust:

```cs
public void OnGet(string searchTerm)
{
    Message = config["Message"];
    Restaurants = restaurantData.GetRestaurantsByName(searchTerm);
}
```

Paraméterként megkapja azt a `searchTerm` változót, aminek a nevét a `name` attribútummal adtuk meg az `input` form elemnél.

Az új data service metódus megkapja ezt a paramétert. Azért volt szükség az üres string vagy `null` lekezelésére a metódusban, mert az oldal első megnyitásakor (tehát amikor még nem keresünk) az `OnGet` egy `null` string bemenetet kap és ilyenkor minden rekordot látni akarunk.

## 2.4. Search string lementése

Azt szeretnénk elérni, hogy a search string megmaradjon a form szöveges mezejében akkor is, miután kész a keresés. Jelenleg ez nem lehetséges, mert az `input` elemnek `value=""` attribútum van beállítva, ami miatt az oldal betöltésekor ez az elem mindig üres lesz.

Ezt úgy érjük el, hogy létrehozunk egy property-t, amit közvetlenül összekötünk a form-al:

```cs
[BindProperty(SupportsGet = true)]
public string SearchTerm { get; set; }
```

A `BindProperty` attribútummal adjuk meg, hogy ezt a property-t összekötöttük az `input` elemmel.

A `SupportsGet = true` paraméterre azért van szükség, mert alapértelmezetten ilyen összekötés csak POST közben lehetséges, de a form-al GET-et triggerelünk.

Frissítjük az `OnGet` metódust, mivel már nem kell neki bemeneti paraméter:

```cs
public void OnGet()
{
    Message = config["Message"];
    Restaurants = restaurantData.GetRestaurantsByName(SearchTerm);
}
```

Az `input` elemet is frissíteni kell:

```html
<input type="search" class="form-control" asp-for="SearchTerm" />
```

A `name` és a `value` attribútumokat lecseréltük az `asp-for="SearchTerm"` kifejezésre. Ez kétirányú összekötést valósít meg az `input` és a model property között, tehát inputként (search string átadása) és outputként (search string kiírása a szöveges mezőbe a keresés után) is működik.

A `@`-al kezdődő C# kódblokkokal ellentétben itt nem szükséges a `Model.` prefix, mivel a compiler tudja, hogy ez az attribútum egy property-t vár.

## 2.5. Detail oldal

### 2.5.1. Detail model és UI

Szeretnénk egy olyan oldalt, ahol az étterem adatai vannak részletezve. Ehhez létrehozunk egy *Detail* Razor page-t. Tartalmazzon egy `Restaurant` típusú property-t, ennek az adatait írjuk ki az oldalra. Ezenkívül érdemes a konstruktorban ennek a property-nek egy mock értéket adni. Így kipróbálhatjuk az oldalt anélkül, hogy `NullReferenceException`-t kapnánk:

```cs
public class DetailModel : PageModel
{
    public Restaurant Restaurant { get; set; }

    public void OnGet()
    {
        Restaurant = new Restaurant();
    }
}
```

A UI-on kiírjuk az étterem adatai:

```html
<h2>@Model.Restaurant.Name</h2>
<div>
    Id: @Model.Restaurant.Id
</div>
<div>
    Location: @Model.Restaurant.Location
</div>
<div>
    Cuisine: @Model.Restaurant.Cuisine
</div>
```

Valamint beszúrunk a végére egy gombot, amivel vissza lehet menni a *List*-re:

```html
<a asp-page="./List" class="btn btn-primary">All Restaurants</a>
```

### 2.5.2. Étterem ID átadása query string-el

A *Detail* page egy étterem ID-t fog kapni a *List* page-től. Ezért módosítani kell az `OnGet` metódust:

```cs
public void OnGet(int restaurantId)
{
    Restaurant = new Restaurant();
    Restaurant.Id = restaurantId;
}
```

Most nincs szükség kétirány kötésre (a paraméter csak inputként szolgál, nem íratjuk ki közvetlenül az oldalra), ezért elég, ha az ID-t paraméterként kapja meg az `OnGet`. A mock adat ID-ját beállítjuk a kapott ID-val, így demózhatjuk a működést.

A *List* page-en a táblázat utolsó oszlopába beszúrunk egy-egy linket, ami a `Detail`-hez irányít át:

```html
<tr>
    <td>@restaurant.Name</td>
    <td>@restaurant.Location</td>
    <td>@restaurant.Cuisine</td>
    <td>
        <a class="btn btn-lg" asp-page="./Detail" asp-route-restaurantId="@restaurant.Id">
            <i class="bi bi-zoom-in"></i>
        </a>
    </td>
</tr>
```

Az `asp-page`-el beállítjuk, melyik page-re akarunk átugrani. A path `./` karakterekkel kezdődik, mert ez egy a `List` page-hez viszonyított relatív path. A `/` karakter önmagában a root-ot jelenti, ami a *Pages* mappa alatti szint (lsd. *Index.cshtml*).

Az `asp-route` dinamikus attribútummal megadhatjuk az átadandó query string-ben szereplő változónevet és annak értékét az `asp-route-{változónév}="{változó értéke}"` formában.

### 2.5.3. Path alapú paraméterátadás query string helyett

A paramétert fogadó oldalt módosíthatjuk úgy, hogy a paramétert csak path-ban fogadja el, query string-ben nem. Ehhez a *Detail.cshtml* `@page` szekcióját kell módosítani:

```cs
@page "{restaurantId:int}"
```

Ezzel megadtuk, hogy a *Detail* page path-on keresztül megkapja a `restaurantId` változó értékét, amit a *List* táblázatában megadott `asp-route-restaurantId` attribútum ad meg. Azt is megszabtuk, hogy az értéknek integer-ré parse-olhatónak kell lennie.

Új path: `https://{localhost}/Restaurants/Detail/{ID}`

Régi path query string-el: `https://{localhost}/Restaurants/Detail?restaurantId={ID}`

### 2.5.4. Lekérdezés ID alapján

Az model és a UI elő vannak készítve, így lecserélhetjük a mock adatot valósra. Ehhez szükségünk van egy metódusra, mely ID alapján keres meg egy entity-t, tehát ki kell egészíteni az `IRestaurantData` interfészt:

```cs
public interface IRestaurantData
{
    //...
    Restaurant GetById(int id);
}
```

Ezt is implementáljuk az `InMemoryRestaurantData`-ban:

```cs
public Restaurant GetById(int id)
{
    return restaurants.SingleOrDefault(r => r.Id == id);
}
```

LINQ-val rákeresünk az ID-ra, és ha nincs ilyen rekord, akkor a default értéket kapjuk vissza, ami `null`.

Beinjektáljuk a data service-t a `Detail` model-be:

```cs
private readonly IRestaurantData restaurantData;

public DetailModel(IRestaurantData restaurantData)
{
    this.restaurantData = restaurantData;
}
```

Valamint felhasználjuk az új metódust a rekord lekérésére:

```cs
public void OnGet(int restaurantId)
{
    Restaurant = restaurantData.GetById(restaurantId);
}
```

### 2.5.5. Hibakezelés

A GET során az átadott paraméterek meg fognak jelenni valamilyen formában az URL-ben. Így a felhasználónak lehetősége van arra, hogy nem létező rekordot kérdezzen le, például ezzel az URL-el:

```
https://{localhost}/Restaurants/Detail/1000
```

1000-es ID-jú rekord nincs, ilyenkor a data service mögött lévő LINQ `null`-t ad vissza, az alkalmazás elszáll `NullReferenceException`-el. Ennek a lehetőségét le kell kezelni a model-ben. A `DetailModel` `OnGet` metódusában megvizsgáljuk, hogy egyáltalán kaptunk-e eredményt a data service-től, és ha nem, akkor egy hibaüzenetet megjelenítő oldalra lépünk:

```cs
public IActionResult OnGet(int restaurantId)
{
    Restaurant = restaurantData.GetById(restaurantId);

    if (Restaurant == null)
    {
        return RedirectToPage("./NotFound");
    }

    return Page();
}
```

Először a `void` visszatérési értéket lecseréljük `IActionResult`-ra, mivel le kell kezelni egy lehetséges másik oldalra lépést is.

Megnézzük, hogy a kapott rekord `null`-e, ha igen, akkor átirányítunk a *NotFound* nevű hibajelző oldalra.

Ha nem, akkor a `Page()` eredménnyel térünk vissza, ami az alapértelmezett *Detail* oldal megjelenítése. 

Most már csak a *NotFound* oldalt kell létrehozni és információval feltölteni:

```html
<h2>Your restaurant was not found.</h2>
<a asp-page="./List" class="btn btn-primary">See All Restaurants</a>
```

Kiírtuk, hogy nincs találat, valamint kiteszünk egy linket, amivel vissza lehet menni a listához.

> Megjegyzés: A *NotFound* mögé nem kell logika, tehát model se. Viszont a *Razor View - Empty* funkcióval létrehozott elemet (ami alapértelmezetten csak egy *.cshtml* fájlt hoz létre) nem ismerte fel a `RedirectToPage` metódus, ezért egy model-el rendelkező oldalt hoztunk létre, és kitöröltük a model-t tartalmazó fájlt, valamint az arra hivatkozó `@model` szekciót a *NotFound.cshtml*-ben. Így már nem jött hiba.

# 3. Rekordok szerkesztése

## 3.1. *Edit* page létrehozása

Létrehozzuk az *Edit* Razor page-t, melynek a model-je megegyezik a *Detail*-ével, vagyis:
- `IRestaurantData` service injektálása
- `Restaurant` típusú property
- Az `OnGet` metódus lekéri az éttermet a kapott ID alapján, és átirányt a *NotFound*-ra, ha nem talált semmit.
- A paramétert path-ban kapja, nem query string-ben.

Ezenkívül a *List*-ben lévő táblázatot kiegészítjük egy oszloppal, amiben egy-egy link van az *Edit*-re:

```html
<td>
    <a class="btn btn-lg" asp-page="./Edit" asp-route-restaurantId="@restaurant.Id">
        <i class="bi bi-pencil-square"></i>
    </a>
</td>
```

## 3.2. *Edit* page form

Létrehozunk egy POST-ot triggerelő form-ot az *Edit* page-en:

```html
<form method="post">
    <input type="hidden" asp-for="Restaurant.Id" />
    <div class="form-group">
        <label asp-for="Restaurant.Name"></label>
        <input asp-for="Restaurant.Name" class="form-control" />
    </div>
    <div class="form-group">
        <label asp-for="Restaurant.Location"></label>
        <input asp-for="Restaurant.Location" class="form-control" />
    </div>
    <div class="form-group">
        <label asp-for="Restaurant.Cuisine"></label>
        <select class="form-control" asp-for="Restaurant.Cuisine" asp-items="Model.Cuisines">
        </select>
    </div>
    <button type="submit" class="btn btn-primary">Save</button>
</form>
```

- Az `asp-for` attribútummal bind-oljuk a szerkesztendő property-ket.
- Az ID-t is át akarjuk adni, mikor rányomunk a submit-ra, de nem szeretnénk, hogy a felhasználók szerkeszteni tudják, ezért az ID input-jának `type="hidden"` attribútumot adtunk.
- A `label`-ek szövegeit is bind-oltuk a property-khez.
- A *Cuisine* beállításához dropdown menüt alkalmazunk, mivel azt szeretnénk, hogy csak az adott véges számú opciókból lehessen értéket adni. Ezért `input` helyett `select`-et használunk.
- Fel kell tölteni a dropdown menüt elemekkel, viszont a page-nek és ezzel az `asp-items` attribútumnak nincs rálátása a Cuisine mögött lévő `enum` elemeire. Ezért a model-ben kell lekérdezni az `enum` adatait, majd lementjük azt a `Cuisines` property-be.

### 3.2.1. Enum kezelése a model-ben

Vanilla HTML5 esetében a `select` komponenst `option` komponensekkel kellene feltölteni, mindegyik `option` komponens az `enum` egy-egy elemét reprezentálná. Ez nem előnyös megoldás, mert ha a jövőben módosulna az `enum`, akkor a form-ot manuálisan frissíteni kellene.

Beinjektáljuk az `IHtmlHelper` service-t:

```cs
private readonly IHtmlHelper htmlHelper;

public EditModel(IRestaurantData restaurantData, IHtmlHelper htmlHelper)
{
    this.restaurantData = restaurantData;
    this.htmlHelper = htmlHelper;
}
```

És ezzel létrehozunk egy listát, amit az `asp-items` már el tud fogadni:

```cs
public IEnumerable<SelectListItem> Cuisines { get; set; }

public IActionResult OnGet(int restaurantId)
{
    Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
    //...
}
```

Így minden egyes GET híváskor automatikusan frissül a dropdown menü elemeinek listája.

## 3.3. POST bind

A módosítás kezeléséhez módosítani kell a data service-t. Először az interfészt bővítjük:

```cs
public interface IRestaurantData
{
    //...
    Restaurant Update(Restaurant updatedRestaurant);
    int Commit();
}
```

A `Commit` arra szolgál, hogy az adatbázison elkövetett változtatásokat egyszerre érvényre juttassa. Jelenleg még nincs ilyenre szükség, ezért még csak egy dummy függvényt implementálunk:

```cs
public int Commit()
{
    return 0;
}
```

Az `Update` kikeresi a megfelelő ID-val rendelkező entity-t és frissíti annak property-jeit a kapott entity property-jeivel (az ID-t leszámítva):

```cs
public Restaurant Update(Restaurant updatedRestaurant)
{
    var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
    if (restaurant != null)
    {
        restaurant.Name = updatedRestaurant.Name;
        restaurant.Location = updatedRestaurant.Location;
        restaurant.Cuisine = updatedRestaurant.Cuisine;
    }
    return restaurant;
}
```

Az *Edit* model-ben kezelnünk kell a submit lenyomását. Ez POST-ot triggerel, ezért implemetálnunk kell az `OnPost` függvényt:

```cs
public IActionResult OnPost()
{
    Restaurant = restaurantData.Update(Restaurant);
    restaurantData.Commit();
    return Page();
}
```

A POST hatására frissítjük a Restaurant property-t, majd meghívjuk a `Commit`-ot, végül visszatérünk az oldallal.

Az `OnGet`-el ellentétben az `OnPost` nem fogad paramétert. Az `OnGet` egy ID-t kapott a `List`-től, és az alapján kellett inicializálnia a `Restaurant` property-t. Az `OnPost`-nak viszont nincs erre szüksége, mivel a submit lenyomásakor már fennáll a kötés a form és a property között.

Ahhoz, hogy a bind működjön, a property-t attribútummal kell dekorálni:

```cs
[BindProperty]
public Restaurant Restaurant { get; set; }
```

Enélkül az `Update` elszáll `NullReferenceException`-nel. Itt nem adtunk át `SupportsGet = true` paramétert, mivel csak POST során van a bind-ra szükségünk.

## 3.4. Az `enum` probléma kezelése

A submit lenyomása után a `Cuisine` dropdown menüje üres lesz, viszont a többi property-nek megmaradt az értéke. Ez azért van, mert ezek a property-k kötve vannak a form-hoz, így az oldal újratöltése során megmaradnak az értékeik (a konstruktor után a `Restaurant` property automatikusan inicializálódik a form-ból kapott értékekkel).

Viszont a dropdown menü elemei nincsenek kötve property-hez, azoknak a listáját a `htmlHelper` service állítja elő. Viszont a listát előállító `GetEnumSelectList` metódust csak az `OnGet`-ben hívtuk, az `OnPost`-ban nem. Minden GET és POST esetén a model újrainicializálódik, újra lefut a kontruktor. Ezért a POST után a dropdown menüelemeket tartalmazó `Cuisines` property `null` lesz.

A megoldás az, hogy az `OnPost`-ban is meg kell hívni a `GetEnumSelectList`-et:

```cs
public IActionResult OnPost()
{
    Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
    //...
}
```

## 3.5. Form input validálása

Jelenleg a form bármilyen bemenetet elfogad, akár egy üres string-et is megadhatunk a string property-knek. Szeretnénk automatikusan ellenőrizni, hogy minden megadott adat valid-e.

Először az entity definíciót kell frissíteni:

```cs
public class Restaurant
{
    //...
    [Required, StringLength(80)]
    public string Name { get; set; }

    [Required, StringLength(255)]
    public string Location { get; set; }
    //...
}
```

Az attribútumokkal megadtuk, hogy mindkettő property-nek értéket kell adni (üres string el lesz utasítva), valamint a maximális megadható string hosszt (80 és 255).

*Edit* `OnPost`:

```cs
public IActionResult OnPost()
{
    if (ModelState.IsValid)
    {
        restaurantData.Update(Restaurant);
        restaurantData.Commit();
    }

    Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
    return Page();
}
```

A `ModelState.IsValid` változóval ellenőrizzük, hogy minden a form-ban átadott adat valid-e. A frissítést és commit-ot csak akkor végezzük el, ha igen.

Így most már ellenőrizzük a bemenetet, de a felhasználó nem kap visszajelzést arról, ha hibás bemenet miatt nem futott le az `Update`. Ezért frissítjük a form-ot:

```html
<div class="form-group">
    <label asp-for="Restaurant.Name"></label>
    <input asp-for="Restaurant.Name" class="form-control" />
    <span class="text-danger" asp-validation-for="Restaurant.Name"></span>
</div>
```

Az `asp-validation-for` attribútummal kiírhatjuk egy property validációjának státuszát, például: `The Name field is required.
`

A többi `input`-nál is beállítjuk ugyanezt.

## 3.6. Post-Redirect-Get (PRG) minta

Az *Edit* jelenlegi működése az, hogy a submit után az oldalon maradunk. Ha ekkor rányomunk a böngészőben a refresh-re, akkor egy figyelmeztetést kapunk, hogy a refresh során újra le fog futni a POST request. Jelen esetben ez nem jelentene problémát, de egyéb felhasználásnál (pl. pénzügyi tranzakció) nem lenne kívánatos dolog.

Ezért ajánlott POST request esetén a Post-Redirect-Get minta használata: A POST request végrehajtása utána ne maradjunk az oldalon, hanem ugorjunk át egy olyan oldalra, ami a POST request eredményét kiírja. Ez a másik oldal GET request-et alkalmaz, így biztonságos a refresh szempontjából. A *Detail* page megfelelő erre a célra.

Módosítsuk az *Edit* `OnPost` metódusát:

```cs
public IActionResult OnPost()
{
    if (ModelState.IsValid)
    {
        restaurantData.Update(Restaurant);
        restaurantData.Commit();
        return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
    }
    //...
}
```

Az `Update` után átirányítunk a *Detail*-re. A metódusban kell megoldanunk a paraméter átadást, ezért a `RedirectToPage` második paramétere egy anonim objektum lesz, benne a `restaurantId`, mivel a `Detail` egy ilyen nevű változót vár.

## 3.7. Új étterem rekord létrehozása

### 3.7.1. Az *Edit* újrafelhasználása

Egy új étterem lementésére szükségünk lenne egy form-ra, de mivel az *Edit* már tartalmaz egy megfelelőt, ezért használjuk fel azt erre a célra is.

Először beszúrunk egy új linket a *List*-be:

```html
<a asp-page=".\Edit" class="btn btn-primary mb-3">Add New</a>
```

Viszont rákattintva a gombra nem történik semmi. Ez azért van, mert az *Edit* egy `restaurantId` nevű integert vár paraméterként, és most nem tudunk ID-t megadni. Ezért az *Edit.cshtml*-t módosítanunk kell, hogy a bemeneti paraméter opcionális legyen:

```cs
@page "{restaurantId:int?}"
```

Ezután az *Edit* `OnGet`-et is módosítani kell:

```cs
public IActionResult OnGet(int? restaurantId)
{
    //...
    if (restaurantId.HasValue)
    {
        Restaurant = restaurantData.GetById(restaurantId.Value);
    }
    else
    {
        Restaurant = new Restaurant();
    }
    //...
}
```

Az opcionális paraméter miatt a `restaurantId` paraméter `null` lesz, ha új éttermet akarunk létrehozni. Ezért a metódus paraméterének nullable integer-nek kell lennie. A keresést akkor végezzük el, ha kaptunk értéket a paraméterben. Ha nem, akkor egy új éttermet inicializálunk.

### 3.7.2. Data service frissítése

Szükségünk van egy olyan metódusra, ami lement egy új éttermet.

Interfész:
```cs
public interface IRestaurantData
{
    //...
    Restaurant Add(Restaurant newRestaurant);
    //...
}
```

Implementáció:
```cs
public Restaurant Add(Restaurant newRestaurant)
{
    restaurants.Add(newRestaurant);
    newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
    return newRestaurant;
}
```

Az új éttermet hozzáadjuk a listához. Viszont az ID-ról nekünk kell gondoskodnunk, ezért megkeressük a legnagyobb ID-t a listában és attól 1-el nagyobbat adunk az új objektumnak.

### 3.7.3. Új étterem lekezelése az *Edit* `OnPost`-ban

A függvény újrastruktúráljuk annak érdekében, hogy ne legyen többszintű `if` blokkok:

```cs
public IActionResult OnPost()
{
    if (!ModelState.IsValid)
    {
        Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
        return Page();
        
    }
    //...
}
```

A dropdown menü elemeit és a default oldalt csak akkor kell kezelni, ha valamelyik adat nem valid. Minden más esetben átirányítunk a PRG minta alapján.

```cs
public IActionResult OnPost()
{
    //...
    if (Restaurant.Id > 0)
    {
        restaurantData.Update(Restaurant);
    }
    else
    {
        restaurantData.Add(Restaurant);
    }
    restaurantData.Commit();
    return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
}
```

Ha az ID nagyobb, mint 0, akkor az már egy létező rekord, így frissíteni kell azt. Ha nem, akkor egy új rekordot kaptunk, azt viszont le kell menteni. Természetesen ehhez kell egy ID konvenció, miszerint annak nagyobbnak kell lennie 0-nál.

A végén átirányítunk a *Detail*-re.

## 3.8. Visszajelzés a sikeres módosításról

Mivel az *Edit* és a *List* is át tud irányítani a *Detail*-re, így nem megállapítható, hogy éppen milyen kontextusban van a *Detail* megnyitva.

Erre egy lehetséges megoldás, hogy az *Edit* `OnPost`-ban az átirányításnál nem csak ID-t, hanem egy üzenetet is átadunk a sikeres műveletről. Ezzel az a probléma, hogy akkor ez az oldal könyvjelzőzhető lenne, és későbbi megnyitáskor is feljönne az üzenet, ami megtévesztő lehet a felhasználónak.

Ehelyett felhasználjuk a `TempData` változót. Ez key-value módon tud adatot tárolni, viszont a felvett adatot csak a következő request-ig tartja meg. Lementjük az üzenetet, mielőtt átlépnénk a *Detail*-re:

```cs
public IActionResult OnPost()
{
    //...
    TempData["Message"] = "Restaurant saved!";
    return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
}
```

Viszont ezt a *Detail* model-nek képesnek kell fogadnia. Létrehozunk egy új property-t a model-ben:

```cs
public class DetailModel : PageModel
{
    //...
    [TempData]
    public string Message { get; set; }
    //...
}
```

A `TempData` attribútummal bind-oltuk a property-t, így a model kikeresi, hogy van-e `Message` key a `TempData`-ban.

A *Detail.cshtml*-ben kiírjuk az üzenetet, ha a property nem `null`:

```html
@if (Model.Message != null)
{
    <div class="alert alert-info">@Model.Message</div>
}
```

Így egy frissítés vagy létrehozás után meg fog jelenni egy visszaigazoló üzenet, viszont egy refresh után eltűnik, mivel a második GET törölte az üzenetet a `TempData`-ból.

# 4. SQL Server és Entity Framework Core

## 4.1. NuGet függőségek

Az *OdeToFood.Data* projektnek a következő függőségeket állítjuk be:
- EntityFramework
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer

Az utolsó kettőnél 5.0.15-ös verzió van megadva, mert az ettől frissebbek már csak .NET 6.0 framework-öt támogatnak.

Ezenkívül az *OdeToFood* projekt is függ a *Microsoft.EntityFrameworkCore.Design v5.0.15* package-től, mert egy későbbi lépésnél (migráció) szükség lesz rá.

## 4.2. Leszármazás `DbContext`-ből

A `DbContext`-ből leszármazott osztályok absztraktálják a kapcsolatot az alkalmazás és az adatbázis között. Ezért az *OdeToFood.Data* projektben létrehozzuk az *OdeToFoodDbContext.cs* fájlt:

```csharp
using Microsoft.EntityFrameworkCore;
using OdeToFood.Core;

namespace OdeToFood.Data
{
    public class OdeToFoodDbContext : DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
```

A `DbSet` típusú objektum fogja modellezni az adatbázis tartalmát.

## 4.3. dotnet Entity Framework Core (EF) CLI

A `dotnet` nem tartalmazza az EF-t, ezért azt külön kell telepíteni:

```
dotnet tool install --global dotnet-ef
```

Ezutána a `dotnet-ef` parancsot használhatjuk.

Menjünk be az *OdeToFood.Data* projekt mappájába és listázzuk ki az implementált `DbContext`-eket:

```
dotnet-ef dbcontext list
```

A parancs build-eli a projektet, majd visszaadja a `DbContext`-et a teljes namespace-el: `OdeToFood.Data.OdeToFoodDbContext`

Ezután használjuk ezt a parancsot:

```
dotnet-ef dbcontext info
```

Ez egy <span style="color:red">No database provider has been configured</span> hibát fog dobni, mivel még nem állítottuk be, hogy milyen adatbázist szeretnénk használni.

## 4.4. Adatbázis service konfigurálása

A Visual Studio-val telepített *LocalDb* adatbázist fogjuk használni. Az adatbázis teljes neve megtalálható az **SQL Server Object Explorer** ablakban: `(localdb)\MSSQLLocalDB`

Az adatbázishoz csatlakozás adatai az ún. *connection string* tartalmazza, amit az *appsettings.json* fájlban tárolunk:

```json
"ConnectionStrings": {
    "OdeToFoodDb": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=OdeToFood;Integrated Security=True"
  }
```
További infó: **[ASP.NET connection string dokumentáció](https://docs.microsoft.com/en-us/previous-versions/aspnet/jj653752(v=vs.110))**

Ezután regisztráljuk az SQL Server service-t a *Startup.cs*-ben:

```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContextPool<OdeToFoodDbContext>(options =>
    {
        options.UseSqlServer(Configuration.GetConnectionString("OdeToFoodDb"));
    });
    //...
}
```

Az `OdeToFoodDbContext` konstruktorát átírjuk úgy, hogy az ősosztály konstruktorának átadjunk egy `DbContextOptions` objektumot:

```cs
public OdeToFoodDbContext(DbContextOptions<OdeToFoodDbContext> options)
    : base(options)
```

## 4.5. Adatbázis migráció

Ha most megint lefuttatjuk a `dotnet-ef dbcontext info` parancsot, akkor egy másik hibaüzenetet kapunk: <span style="color:red">Unable to create an object of type 'OdeToFoodDbContext'.</span>

Ez azért van, mert a szükséges infók (pl. connection string, service regisztrálása) nem itt vannak definiálva, hanem a webapplikáció projektben. Viszont megadhatjuk a startup projektet: `dotnet-ef dbcontext info -s ..\OdeToFood\OdeToFood.csproj`

Megkapjuk az adatbázis adatait:

```
Provider name: Microsoft.EntityFrameworkCore.SqlServer
Database name: OdeToFood
Data source: (localdb)\MSSQLLocalDB
Options: MaxPoolSize=128
```

Elkezdjük a migrációt: `dotnet-ef migrations add initialcreate -s ..\OdeToFood\OdeToFood.csproj`

A migrációt az `initialcreate` címkével láttuk el, tekintve hogy ez az első. Itt is meg kellett adni a startup projektet.

Az *OdeToFood.Data* projektben megjelent egy *Migrations* mappa, amiben az adatbázis beállítások találhatók meg, amik az eddigi munkánk alapján generálódtak.

Legutoljára alkalmazzuk a migrációt: `dotnet-ef database update -s ..\OdeToFood\OdeToFood.csproj`

Frissítve az **SQL Server Object Explorer** ablakot, valamint lenyitva a *(localdb)\MSSQLLocalDB\Databases* mappát, akkor ott látható az *OdeToFood* adatbázis és annak minden beállítása (pl. columns).

## 4.6. SQL Server data access service implementálása

Az új data service szintén az `IRestaurantData` interfészt implementálja, követve a *Dependency Injection* elvét. Az adatbázist az `OdeToFoodDbContext`-en keresztül fogja elérni:

```cs
public class SqlRestaurantData : IRestaurantData
{
    private readonly OdeToFoodDbContext db;

    public SqlRestaurantData(OdeToFoodDbContext db)
    {
        this.db = db;
    }
    //...
}
```

Implementáljuk az interfészt:

```cs
public Restaurant Add(Restaurant newRestaurant)
{
    //A DbContext.Add() metódussal hozzáadunk egy új entity-t a DbSet-hez
    db.Add(newRestaurant);
    return newRestaurant;
}

public int Commit()
{
    //Nem történik változás az adatbázisban, amíg a SaveChanges() nem fut le
    return db.SaveChanges();
}

public Restaurant Delete(int id)
{
    //Megkeressük ID alapján, ha megvan, akkor töröljük a DbSet-ből
    var restaurant = GetById(id);
    if (restaurant != null)
    {
        db.Restaurants.Remove(restaurant);
    }
    return restaurant;
}

public Restaurant GetById(int id)
{
    //A Find() primary key alapján keres entity-t
    return db.Restaurants.Find(id);
}

public IEnumerable<Restaurant> GetRestaurantsByName(string name)
{
    //u.a. mint az InMemory implementációban
    var query = from r in db.Restaurants
                where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                orderby r.Name
                select r;
    return query;
}

public Restaurant Update(Restaurant updatedRestaurant)
{
    /*
    * Az Attach() és a State lementik, hogy ez az entity megváltozott
    * és a Commit() során az adatbázisnak frissíteni kell a property-jeit
    */
    var entity = db.Restaurants.Attach(updatedRestaurant);
    entity.State = EntityState.Modified;
    return updatedRestaurant;
}
```

Most már csak annyi a dolgunk, hogy a *Startup.cs*-ben lecseréljük az `InMemoryRestaurantData` service regisztrálását, és helyette az `SqlRestaurantData`-t használjuk:

```cs
public void ConfigureServices(IServiceCollection services)
{
    //...
    services.AddScoped<IRestaurantData, SqlRestaurantData>();
    //...
}
```

Az `AddSingleton` helyett az `AddScoped` metódust használjuk, így a service csak egy request erejéig fog fennmaradni.

Az alkalmazás elindítása után üres lesz az étterem-lista, mert az adatbázis is üres. Most már a változtatások az app újraindítása után is megmaradnak.

# 5. UI fejlesztése

## 5.1. A *_Layout.cshtml* működése

Az ASP.NET szempontjából a *_Layout.cshtml* nem egy page, mert:
- nincs neki egy `@page` direktívája
- nem lehet egy route célpontja

A `_` karakterrel kezdődő fájlok nem jeleníthetők meg magukban, hanem csak kisegítő fájlok.

A page-ek tartalma ott fog megjelenni, ahol a `@RenderBody()` hívás megtörténik.

Viszont előfordulhat, hogy egy page-el nem csak a body-ba szeretnénk tartalmat tenni, hanem pl. a footer-be is.

A *List*-ben módosítsuk az oldal végén lévő üzenet kiírást:

```html
@section footer {
    <div>@Model.Message</div>
}
```

Erre az elemre *footer* néven tudunk hivatkozni. Hívjuk meg ezt a *Layout*-ban:

```html
<footer class="border-top footer text-muted">
    <div class="container">
        @RenderSection("footer", required: false)
    </div>
    @*...*@
</footer>
```

Így beinjektáltunk tartalmat a footer-be. A `container` osztály egy [Bootstrap alapelem](https://getbootstrap.com/docs/5.1/layout/containers/#default-container).

## 5.2. *Delete* page a default layout nélkül

Létrehozzuk a *Delete* Razor page-t. A model-ben a GET egy ID-t fogad, és az alapján lement egy entity-t:

```cs
public IActionResult OnGet(int restaurantId)
{
    Restaurant = restaurantData.GetById(restaurantId);
    if (Restaurant == null)
    {
        return RedirectToPage("./NotFound");
    }
    return Page();
}
```

A POST pedig törli az entity-t, és utána visszamegy a *List*-re a PRG minta szerint:

```cs
public IActionResult OnPost(int restaurantId)
{
    var restaurant = restaurantData.Delete(restaurantId);
    restaurantData.Commit();

    if (restaurant == null)
    {
        return RedirectToPage("./NotFound");
    }

    TempData["Message"] = $"{restaurant.Name} deleted";
    return RedirectToPage("./List");
}
```

Az `OnPost` egy ID-t fogad, mivel a form-nál nem lesz megadva az `action` attribútum, így a POST ugyanazzal az URL-el fog hívódni, tehát a végén ott lesz az ID.

A UI-nál beállítjuk, hogy fogadunk egy ID-t, valamint hogy nem használjuk fel a default layout-ot:

```cs
@page "{restaurantId}"
@model OdeToFood.Pages.Restaurants.DeleteModel
@{
    Layout = null;
}
```

Ezután egy HTML5 boilerplate-ben elhelyezzük a form-ot:

```html
<h2>Delete!</h2>

<div class="alert alert-danger">
    Are you sure you want to delete @Model.Restaurant.Name?
</div>

<form method="post">
    <button type="submit" class="btn btn-danger">Yes!</button>
    <a asp-page="List" class="btn btn-primary">Cancel</a>
</form>
```

A form nem lesz stilizálva, mivel a default layout importálja a Bootstrap-et, így hiába használtunk Bootstrap osztályokat, azok nem fognak érvényre jutni.

Végül a *List* táblázatában beszúrunk egy oszlopot, amiben a linkek a *Delete*-re mutatnak:

```html
<td>
    <a class="btn btn-lg" asp-page="./Delete" asp-route-restaurantId="@restaurant.Id">
        <i class="bi bi-trash"></i>
    </a>
</td>
```

## 5.3. *_ViewStart* és *_ViewImports*

Ha a *Delete.cshtml*-ben töröljük a `Layout = null` sort, valamint a struktúrából csak a `body` elemeit hagyjuk meg, akkor megint érvényre jut a default layout.

Ez azért van, mert a *_ViewStart* fog lefutni minden page renderelése előtt, és a *_ViewStart* a default layout-ot állítja be. Ezt a beállítást tudtuk felülírni a page szintjén az előbb.

Ezenfelül a *_ViewImports* beállítja a következőket:
- `using` parancsok, mely namespace-eket használjuk
- default namespace
- tag helper importálása
  - `@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers`: IntelliSense az `asp-<...>` szerű attribútumoknál

## 5.4. Partial View

A Partial View egy olyan page, ami magában nem renderelhető, viszont más page meg tudja hívni (úgy, mint a *_Layout.cshtml*).

Nincs saját model-je, viszont hívás során képes kinti model-t fogadni és azt felhasználni. A Partial View úgy viselkedik, mint egy metódus UI szemszögből, a UI elemeket kisebb egységekre tudjuk felbontani.

A példában a *List* táblázatának elemeit fogjuk kiváltani partial view-al.

Először létrehozzuk a *_Summary* page-t. A konvenciót betartjuk, miszerint az önmagában nem megjeleníthető elemek nevét `_` karakterrel kezdjük.

Kitöröljük a model fájlt (mert a partial view-nak nem lehet), majd a *_Summary.cshtml*-be beillesztjük ezt:

```html
@using OdeToFood.Core
@model Restaurant
```

A `using` direktívával megmondjuk, hogy melyik namespace-re akarunk rálátni innen, a `model`-el megadjuk, hogy milyen típusú objektumot várunk akkor, amikor meghívják a partial view-t.

Létrehozunk egy [Bootstrap Card](https://getbootstrap.com/docs/5.1/components/card/) elemet, majd feltöltjük azokkal az adatokkal és linkekkel, melyek a táblázatban is ott voltak:

```html
<div class="card">
    <h5 class="card-header">@Model.Name</h5>
    <div class="card-body">
        <p class="card-text">Location: @Model.Location</p>
        <p class="card-text">Cuisine: @Model.Cuisine</p>
    </div>
    <div class="card-footer">     
        <a class="btn btn-lg" asp-page="./Detail" asp-route-restaurantId="@Model.Id">
            <i class="bi bi-zoom-in"></i>
        </a>             
        <a class="btn btn-lg" asp-page="./Edit" asp-route-restaurantId="@Model.Id">
            <i class="bi bi-pencil-square"></i>
        </a>              
        <a class="btn btn-lg" asp-page="./Delete" asp-route-restaurantId="@Model.Id">
            <i class="bi bi-trash"></i>
        </a>       
    </div>
</div>
```

A *List*-ben kitöröljük a táblázatot, és meghívjuk a partial view-t:

```cs
@foreach (var restaurant in Model.Restaurants)
{
    <partial name="_Summary" model="restaurant" />
}
```

Megadjuk, hogy melyik partial view definíció akarjuk használni, valamint a model entity-t is.

## 5.5. View Component

A partial view-hoz hasonlóan a view component is egy csonka Razor page. Míg az előbbivel a UI elemeit szervezhettük kisebb egységekbe, addig az utóbbival kisebb model logikát implementálhatunk UI komponens nélkül.

A feladat az, hogy a footer-ben mindig legyen kiírva, hogy az adatbázisban mennyi rekord van. Ennek kezelésére létrehozzuk a *RestaurantCountViewComponent.cs* osztályt a *(project root)\ViewComponents* mappában (a *ViewComponents* a *Pages* mappával van egy szinten):

```cs
public class RestaurantCountViewComponent : ViewComponent
{
    private readonly IRestaurantData restaurantData;

    public RestaurantCountViewComponent(IRestaurantData restaurantData)
    {
        this.restaurantData = restaurantData;
    }

    public IViewComponentResult Invoke()
    {
        var count = restaurantData.GetCountOfRestaurants();
        return View(count);
    }
}
```

Az osztály a `ViewComponent`-ből származik le, a konstruktora megkapja az `IRestaurantData` service-t. Az `Invoke` metódus fog lefutni a komponens meghívása során, benne lekérdezzük a rekordok számosságát, majd az összeget visszaadjuk egy `IViewComponentResult` objektumban.

Ehhez a data service-t módosítani kell, hogy realizálja a `GetCountOfRestaurants` függvényt.

Interfész:

```cs
public interface IRestaurantData
{
    //...
    int GetCountOfRestaurants();
}
```

*InMemoryRestaurantData.cs*:

```cs
public int GetCountOfRestaurants()
{
    return restaurants.Count();
}
```

*SqlRestaurantData.cs*:

```cs
public int GetCountOfRestaurants()
{
    return db.Restaurants.Count();
}
```

Most meg kell jeleníteni ennek a model-nek a kimenetét. Ehhez létrehozunk egy Razor page-t itt: *(project root)\Pages\Shared\Components\RestaurantCount\Default.cshtml*. Ezt az elnevezési konvenciót meg kell tartani, mert az ASP.NET csak így ismeri fel az elemeket.

Töröljük a model fájlt, majd módosítjuk a *Default.cshtml* tartalmát:

```cs
@model int

<div>
    There are @Model restaurants here. <a asp-page="/Restaurants/List">See them all!</a>
</div>
```

Bemenetként egy egyszerű integert várunk, majd azt kiírjuk.

A *_ViewImports.cshtml*-ben felvesszük a tag helper listára a projektet, így az IntelliSense látni fogja az újonnan létrehozott elemet:

```
@using OdeToFood
@namespace OdeToFood.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

@*Ez az új tag helper amivel láthatjuk az új view component elemet:*@
@addTagHelper *, OdeToFood
```

A *_Layout.cshtml*-ben a footer-nél beszúrjuk az új elemet:

```html
<footer class="border-top footer text-muted">
    <div class="container">
        <vc:restaurant-count></vc:restaurant-count>
    </div>
    @*...*@
</footer>
```

A fájl és mappa elnevezési és hierarchia konvenciók betartása, valamint az új tag helper felvételével az IntelliSense felismerte az új elemet. A NagyKicsi betűs elnevezést automatikusan lecseréli az ASP.NET ú.n. kebab case-re: kis betűk, szavak kötőjellel elválasztva.

## 5.6. Partial View és View Component megkülönböztetése

- A partial view függ az őt hívó page-től, mert onnan kapja a model bemenetet.
- A view component független az őt hívó page-től, saját magának kéri le a megjelenítendő adatot, nem kapja.

## 5.7. Scaffolding

Automatikus CRUD parancs implementálás az entity és a DbContext alapján. Gyors prototípus előállításra jó, hibák lehetnek benne.

# 6. Kliens oldali JavaScript és CSS kód integrálása

## 6.1. Statikus fájlok kezelése

Egy page-en beszúrunk egy képet:

```html
<footer class="border-top footer text-muted">
    @*...*@
    <div class="container">
        <img src="/restaurant.png" />
    </div>
</footer>
```

Statikus fájlok esetében a root a *wwwwroot* mappát jelenti, tehát a statikus fájlokat ebbe a mappába kell tenni.

## 6.2. Kliens oldali validálás

Az entity-nél már implementáltunk validálást, viszont az csak szerver oldali, tehát rá kell nyomni a submit-ra, hogy kapjunk hibaüzenetet. 

Az ASP.NET automatikusan beleteszi a projektbe a kliens oldali validáló szkripteket (*jquery-validation* és *jquery-validation-unobtrusive*), viszont a *_Layout.cshtml* ezeket nem hivatkozza. A hivatkozást a *\Pages\Shared\_ValidationScriptsPartial.cshtml* partial view page tartalmazza.

A *_Layout.cshtml* végén található egy `@RenderSection("Scripts", required: false)` sor, tehát a Razor page definiálhat egy *Scripts* szekciót, amiben meghívjuk a *_ValidationScriptsPartial.cshtml* partial view-t. Így csak a szükséges oldalaknál fog ez a kettő library betöltődni.

Kliens oldali validálásra az *Edit*-ben van szükség. A page aljára beszúrjuk:

```html
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

Így amikor az *Edit*-en vagyunk, akkor a szekción keresztül meghívjuk a validálást. Ekkor a hibaüzenet már a submit előtt megjelenik.

## API controller használata

> A scaffolding nem működött -> doksi

