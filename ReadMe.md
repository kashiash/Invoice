# eXpressApp Framework przyjacielem programisty

Proces tworzenia  oprogramowania składa się z różnych etapów, z których niektóre potrafimy robić dniami i nocami np. 3 doby na pizzy i redbulu, oraz takie które odkładamy jak się da i najchętniej delegujemy junior programmer’owi. W efekcie powstają smutne historię i w kolejnej firmie już nasz były junior, opowiada, że zajmował się cały czas np. nudnymi CRUD’ami, albo czymś jeszcze nudniejszym. 

Standardowy proces tworzenia oprogramowania stawia przed programistami następujące wyzwania:
*	 Najprostsze czynności jak przeglądanie czy przechowywanie danych są czasochłonne. Programiści musza dbać o każdy aspekt tworzonej aplikacji – od zarządzania danymi na poziomie serwerów danych, po dostarczenie edytorów do każdego edytowanego pola.
*	Im bardziej złożony system, tym więcej kodu, tym więcej nieuchronnych błędów. Do celów testowych potrzeba znaczną ilość czasu i zasobów ludzkich.
*	Utrzymanie tak stworzonego systemu nie jest trywialne. Nawet trzymając się wszelkich zasad programowania, wiele zadań będzie wymagało modyfikacji aplikacji w wielu miejscach. Jej rozbudowa jest kosztowna i koszt ten rośnie wraz ze złożonością systemu.
Oczywiście niniejsze podejście ma tez swoje zalety:
*	Każdy aspekt powstającego sytemu jest w pełni kontrolowany przez programistów. Nie są uzależnieni od ograniczeń czy nawet błędów zewnętrznych bibliotek. Wszystko co stworzyli mogą modyfikować i poprawiać w prostszy sposób.
*	Programiści mogą optymalizować system wg własnych potrzeb, co jest trudne do osiągnięcia bazując na zewnętrznych rozwiązaniach.
*	Aplikacje nie musza być tworzone wg zasad wymaganych przez zewnętrzne narzędzia/biblioteki.

Niekiedy powyższe rozwiązanie jest jedynym wyjściem aby stworzyć właściwy system, często jednak wykonujemy systemy w których pewne funkcjonalności powtarzają się i faktycznie robienie tego samego w kółko zaczyna być nużące.

Od lat powstają narzędzia, które próbują wyeliminować powtarzalne elementy systemu, które prawie zawsze robi się w podobny sposób niezależnie od tego czy jest to aplikacja do wystawiania faktur, czy program do diagnozowania i leczenia raka. Narzędzia tego typu zwane kiedyś RAD (Rapid Application Development) np. Power Builder, Clarion, Power Apps i wiele innych, w różnym stopniu pozwalały programistom na elastyczność podczas procesu tworzenia aplikacji. Jedne wymagały trzymania się konkretnych zasad i pozwalały na tworzenie aplikacji o dość ograniczonej funkcjonalności, inne pozwalały na większa elastyczność, nie mniej jednak bardzo często kończyło się na egzotycznych trikach by osiągnąć zamierzony cel. O skuteczności tych narzędzi świadczą systemy jakie powstały choćby w Polsce m.in. cała seria WaPro WF-MAG (KaPer,Gang,Fakir) czy Comarch ERP XL stworzone z wykorzystaniem Clarion’a, czy produkty rodziny Simple.ERP, tworzone za pomocą Power Builder’a i wiele innych. 
Z czasem narzędzia te zaczęły tracić przewagę z powodu rozwoju języków obiektowych i pojawiania się bibliotek wspomagających programistów w każdym możliwym aspekcie ich pracy.

Jednym z takich jest Devexpress eXpressApp Framework (XAF). (Niestety nie jest to narzędzie darmowe, ale dostępna jest wersja testowa, a efekt końcowy jest wart ceny licencji - w końcu to jedynie  miesięczna pensja junior developera).

XAF opiera się na architekturze MVC. Dane przechowujemy w bazie danych np. MS SQL (XAF wspiera kilkanaście serwerów baz danych ). Komunikacja z baza danych jest poprzez ORM (XPO lub Entity Framework Core). ORM służy do mapowania struktur tabel bazy danych na klasy w modelu aplikacji. Zadeklarowane klasy modelujące naszą dziedzinę biznesową automatycznie są konwertowane na Widoki (ListView, DetailView) , które pozwalają na dodawanie, modyfikację czy przeglądanie danych (nudne CRUD’y poszły się …)

ListView wyświetlają  kolekcje danych, pozwalają je sortować i przeszukiwać z wykorzystaniem zaawansowanych metod filtrowania.

DetailView pozwalają na prace z pojedynczym obiektem (rekordem danych) wyświetlając dane w odpowiednich edytorach. Wykorzystywane są do dodawania i edycji danych.
DashboardView pozwala grupować wiele innych widoków na jednym oknie.

#### Klasa Biznesowa

Model biznesowy definiujemy za pomocą klas, dla których zostaną utworzone struktury tabel i relacji w bazie danych i jednocześnie zostaną utworzone widoki używane w interfejsie aplikacji.

Klasy możemy stworzyć na 3 sposoby:
1.	Model First - Definiując klasy i powiązania w dedykowanym Edytorze Modelu (XPO Data Model Designer) i generując klasy na podstawie tego modelu.
2.	Database First – importując struktury z istniejącej bazy danych do Edytora Modelu i następnie wygenerowanie klas.
3.	Code First – Deklarując klasy bezpośrednio w kodzie.
Osobiście preferuję wariant 3-ci – czyli klasy definiowane bezpośrednio w kodzie.

Potrzebujemy następujące klasy i ich pola:

![Diagram powiązań pomiędzy tabelami](Diagram2.png)


##### Klient
```csharp
[DefaultClassOptions]
   public class Customer : XPObject
   {
       public Customer(Session session) : base(session)
       { }


       string postalCode;
       string city;
       string street;
       string customerName;
       string vatNumber;
       string symbol;

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Symbol
       {
           get => symbol;
           set => SetPropertyValue(nameof(Symbol), ref symbol, value);
       }

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string VatNumber
       {
           get => vatNumber;
           set => SetPropertyValue(nameof(VatNumber), ref vatNumber, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string CustomerName
       {
           get => customerName;
           set => SetPropertyValue(nameof(CustomerName), ref customerName, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string Street
       {
           get => street;
           set => SetPropertyValue(nameof(Street), ref street, value);
       }


       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string City
       {
           get => city;
           set => SetPropertyValue(nameof(City), ref city, value);
       }
       
       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string PostalCode
       {
           get => postalCode;
           set => SetPropertyValue(nameof(PostalCode), ref postalCode, value);
       }

   }
```


##### Produkt

```csharp
[DefaultClassOptions]
public class Product : BaseObject
{
    public Product(Session session) : base(session)
    { }


    string notes;
    string gTIN;
    string productName;
    string symbol;

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Symbol
    {
        get => symbol;
        set => SetPropertyValue(nameof(Symbol), ref symbol, value);
    }


    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string ProductName
    {
        get => productName;
        set => SetPropertyValue(nameof(ProductName), ref productName, value);
    }


    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string GTIN
    {
        get => gTIN;
        set => SetPropertyValue(nameof(GTIN), ref gTIN, value);
    }

    
    [Size(SizeAttribute.Unlimited)]
    public string Notes
    {
        get => notes;
        set => SetPropertyValue(nameof(Notes), ref notes, value);
    }

}
```

##### Faktura

```csharp
[DefaultClassOptions]
   public class Invoice : BaseObject
   {
       public Invoice(Session session) : base(session)
       { }


       string notes;
       decimal brutto;
       decimal vat;
       decimal netto;
       Customer customer;
       DateTime dueDate;
       DateTime invoiceDate;
       string invoiceNumber;

       [Size(SizeAttribute.DefaultStringMappingFieldSize)]
       public string InvoiceNumber
       {
           get => invoiceNumber;
           set => SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value);
       }



       public DateTime InvoiceDate
       {
           get => invoiceDate;
           set => SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value);
       }


       public DateTime DueDate
       {
           get => dueDate;
           set => SetPropertyValue(nameof(DueDate), ref dueDate, value);
       }


       public Customer Customer
       {
           get => customer;
           set => SetPropertyValue(nameof(Customer), ref customer, value);
       }


       public decimal Netto
       {
           get => netto;
           set => SetPropertyValue(nameof(Netto), ref netto, value);
       }


       public decimal Vat
       {
           get => vat;
           set => SetPropertyValue(nameof(Vat), ref vat, value);
       }


       public decimal Brutto
       {
           get => brutto;
           set => SetPropertyValue(nameof(Brutto), ref brutto, value);
       }

       
       [Size(SizeAttribute.Unlimited)]
       public string Notes
       {
           get => notes;
           set => SetPropertyValue(nameof(Notes), ref notes, value);
       }
   }
   
   
     public class InvoiceItem : BaseObject
    {
        public InvoiceItem(Session session) : base(session)
        { }


        Invoice invoice;
        decimal brutto;
        decimal vat;
        decimal netto;
        decimal unitPrice;
        decimal quantity;
        Product product;

        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }


        [Association]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        public decimal Quantity
        {
            get => quantity;
            set => SetPropertyValue(nameof(Quantity), ref quantity, value);
        }


        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }


        public decimal Netto
        {
            get => netto;
            set => SetPropertyValue(nameof(Netto), ref netto, value);
        }

        public decimal Vat
        {
            get => vat;
            set => SetPropertyValue(nameof(Vat), ref vat, value);
        }

        
        public decimal Brutto
        {
            get => brutto;
            set => SetPropertyValue(nameof(Brutto), ref brutto, value);
        }

    }
```


#### Relacje

XPO wspiera 3 typy relacji pomiędzy obiektami: 
*	jeden do wielu 1-N
*	Jeden do Jednego 1-1
*	Wiele do Wielu N-M

W naszym przypadku mamy do czynienia z następującymi relacjami:

* Klient może mieć dowolną liczbę faktur 1-N
* Faktura ma co najmniej jedna pozycję 1-N
* Każda pozycja jest w relacji do Produktu. (Produkt może być na wielu pozycjach) 1-N.

W fakturze do pola Customer dodajemy adnotację Association oraz Aggregated

```csharp
[Association,Aggregated]
public Customer Customer
{
    get => customer;
    set => SetPropertyValue(nameof(Customer), ref customer, value);
}
```


W klasie klienta dodajemy kolekcję do wyświetlania listy faktur

```csharp
[Association]
public XPCollection<Invoice> Invoices
{
    get
    {
        return GetCollection<Invoice>(nameof(Invoices));
    }
}
```


w fakturze dodajemy kolekcję Pozycji faktury i oznaczamy je odpowiednimi adnotacjami:
```csharp
[Association]
public XPCollection<InvoiceItem> Items
{
    get
    {
        return GetCollection<InvoiceItem>(nameof(Items));
    }
}
```
A w pozycji dodajemy powiązanie do faktury:

```csharp
[Association]
public Invoice Invoice
{
    get => invoice;
    set => SetPropertyValue(nameof(Invoice), ref invoice, value);
}
```

Wiadomo, że program lepiej wygląda z danymi, wiec wygenerujemy nieco danych testowych wykorzystując pakiet Bogus.

W pliku Updater.cs dodajemy kod który wywoła metody wpisujące dane testowe:


```csharp
public override void UpdateDatabaseAfterUpdateSchema()
{
    base.UpdateDatabaseAfterUpdateSchema();

    PrepareTestData();
    ObjectSpace.CommitChanges(); 
}


  private void PrepareTestData()
        {
            var cusFaker = new Faker<Customer>("pl")
                .CustomInstantiator(f => ObjectSpace.CreateObject<Customer>())

                .RuleFor(o => o.Notes, f => f.Company.CatchPhrase())
                .RuleFor(o => o.CustomerName, f => f.Company.CompanyName())

                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.PostalCode, f => f.Address.ZipCode())
                .RuleFor(o => o.Street, f => f.Address.StreetName());
            cusFaker.Generate(100);


            var prodFaker = new Faker<Product>("pl")

            .CustomInstantiator(f => ObjectSpace.CreateObject<Product>())
                .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
                .RuleFor(o => o.Notes, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.Symbol, f => f.Commerce.Product())
                .RuleFor(o => o.GTIN, f => f.Commerce.Ean13());

            prodFaker.Generate(100);


            var customers = ObjectSpace.GetObjectsQuery<Customer>(true).ToList();


            var orderFaker = new Faker<Invoice.Module.BusinessObjects.Invoice>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Invoice.Module.BusinessObjects.Invoice>())
                .RuleFor(o => o.InvoiceNumber, f => f.Random.Int().ToString())
                .RuleFor(o => o.InvoiceDate, f => f.Date.Past(20))
                .RuleFor(o => o.DueDate, f => f.Date.Past(2))
                .RuleFor(o => o.Customer, f => f.PickRandom(customers));
            var orders = orderFaker.Generate(customers.Count * 10);

            var products = ObjectSpace.GetObjectsQuery<Product>(true).ToList();

            var itemsFaker = new Faker<InvoiceItem>()
            .CustomInstantiator(f => ObjectSpace.CreateObject<InvoiceItem>())
                .RuleFor(o => o.Invoice, f => f.PickRandom(orders))
                .RuleFor(o => o.Product, f => f.PickRandom(products))

                .RuleFor(o => o.Quantity, f => f.Random.Decimal(0.01M, 100M))
                .RuleFor(o => o.UnitPrice, f => f.Random.Decimal(0.01M, 100M));
            var items = itemsFaker.Generate(orders.Count * 10);
        }
```


Kompilujemy i uruchamiamy program. Do dyspozycji mamy wersje WinForms lub Blazor. W zależności od tego co wybierzemy naszym oczom pojawi się wersja Windowsowa:

![](winform1.png)

Jak widać dostajemy z autoamtu mozliwośc prostego wyszikiwania w liście w sposób znany choćby z programu Excel. Jak i bardzie zaawansowany edytor filtrów:

![](filtr1.png)

lub Webowa:

![](blazor1.png)
