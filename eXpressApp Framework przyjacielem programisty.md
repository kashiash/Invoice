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
W serii kilku artykułów postaram się pokazać proces tworzenia prostej aplikacji, która pozwoli nam na prowadzenie ewidencji klientów, wystawianie faktur czy ewidencjonowania wpłat za nie. Wiem, ze temat oklepany, już niejeden taki program zrobiliście, ale dzięki temu macie szanse porównać jaka ilość czasu trzeba poświęcić na zrobienie podobnej funkcjonalności z wykorzystaniem XAF.  (Jeśli artykuły spotkają się z zainteresowanie czytelników, możemy zmodyfikować czy rozwinąć zakres funkcjonalny powstającej aplikacji).
Jednocześnie informuję że miejscami będę stosował pewne uproszczenia i proszę mnie nie linczować, jeśli gdzieś architektonicznie pójdę na skróty.

#### Klasa Biznesowa

Model biznesowy definiujemy za pomocą klas, dla których zostaną utworzone struktury tabel i relacji w bazie danych i jednocześnie zostaną utworzone widoki używane w interfejsie aplikacji.
Klasy możemy stworzyć na 3 sposoby:
1.	Model First - Definiując klasy i powiązania w dedykowanym Edytorze Modelu (XPO Data Model Designer) i generując klasy na podstawie tego modelu.
2.	Database First – importując struktury z istniejącej bazy danych do Edytora Modelu i następnie wygenerowanie klas.
3.	Code First – Deklarując klasy bezpośrednio w kodzie.
Osobiście preferuję wariant 3ci – czyli klasy definiowane bezpośrednio w kodzie.

Potrzebujemy następujące klasy i ich pola:

![AAAA](Diagram2.png)

Klient (Symbol, NIP, Nazwa, Ulica, KodPocztowy, Miejscowość, Uwagi)
Produkt(Symbol, GTIN, Nazwa, Uwagi)
Faktura(Numer, Klient, DataFaktury, DataPlatnosci, WartoscNetto, WartoscBrutto, WartoscVat)

Ze względu na estetykę końcowego kodu klasy zdefiniuję po pogańsku. Nazwy polsko-angielskie KlientListViewController trochę dziwnie wyglądają 😉.

##### Klient


##### Produkt


##### Faktura


#### Relacje

XPO wspiera 3 typy relacji pomiędzy obiektami: 
*	jeden do wielu 1-N
*	Jeden do Jednego 1-1
*	Wiele do Wielu N-M

Klient może mieć dowolną liczbę faktur 1-N
Faktura ma co najmniej jedna pozycję 1-N
Każda pozycja jest w relacji do Produktu. (Produkt może być na wielu pozycjach) 1-N

