using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using GUS.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Customer : CustomBaseObject, IOrganizationGus, IAddressGus
    {
        public Customer(Session session) : base(session)
        { }


        string phone;
        string email;
        Segment segment;
        string notes;
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



        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Phone
        {
            get => phone;
            set => SetPropertyValue(nameof(Phone), ref phone, value);
        }


        public Segment Segment
        {
            get => segment;
            set => SetPropertyValue(nameof(Segment), ref segment, value);
        }

        [Association, DevExpress.Xpo.Aggregated]
        [DetailViewLayoutAttribute("InvoicesNotes", LayoutGroupType.TabbedGroup, 100)]
        public XPCollection<Invoice> Invoices
        {
            get
            {
                return GetCollection<Invoice>(nameof(Invoices));
            }
        }

        [DetailViewLayoutAttribute("InvoicesNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("Customer-Payments"), DevExpress.Xpo.Aggregated]
        public XPCollection<Payment> Payments
        {
            get
            {
                return GetCollection<Payment>(nameof(Payments));
            }
        }

        [DetailViewLayoutAttribute("InvoicesNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("Customer-Projects"), DevExpress.Xpo.Aggregated]
        public XPCollection<Project> Projects
        {
            get
            {
                return GetCollection<Project>(nameof(Projects));
            }
        }
        [DetailViewLayoutAttribute("InvoicesNotes", LayoutGroupType.TabbedGroup, 100)]

        [Association("Customer-AssignedFileData"), DevExpress.Xpo.Aggregated]
        public XPCollection<AssignedFileData> Files
        {
            get
            {
                return GetCollection<AssignedFileData>(nameof(Files));
            }
        }

        [DetailViewLayoutAttribute("InvoicesNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        #region GUS
        RegistrationAuthority registrationAuthority;
        FoundingBody organZalozycielski;
        PropertyForm formaWlasnosci;
        FinancingForm formaFinansowania;
        SpecificLegalForm szczegolnaFormaPrawna;
        BasicLegalForm podstawowaFormaPrawna;
        RegistryType registryType;
        DateTime dateOfChangeOccurrence;
        DateTime dateOfEntryToRegisterOfRecords;
        DateTime dateOfEntryToRegon;
        DateTime dateOfCommencementBusiness;
        DateTime dateOfCreation;
        string numberInRegisterOfRecords;
        CountryGus krajGus;
        ProvinceGus wojewodztwoGus;
        CommuneGus gminaGus;
        CityGus miastoPocztyGus;
        CityGus miastoGus;
        StreetGus ulicaGus;
        CountyGus powiatGus;

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Podstawowa forma prawna")]
        public BasicLegalForm BasicLegalForm
        {
            get => podstawowaFormaPrawna;
            set => SetPropertyValue(nameof(BasicLegalForm), ref podstawowaFormaPrawna, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Szczególna forma prawna")]
        public SpecificLegalForm SpecificLegalForm
        {
            get => szczegolnaFormaPrawna;
            set => SetPropertyValue(nameof(SpecificLegalForm), ref szczegolnaFormaPrawna, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Forma finansowania")]
        public FinancingForm FinancingForm
        {
            get => formaFinansowania;
            set => SetPropertyValue(nameof(FinancingForm), ref formaFinansowania, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Forma własności")]
        public PropertyForm PropertyForm
        {
            get => formaWlasnosci;
            set => SetPropertyValue(nameof(PropertyForm), ref formaWlasnosci, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Organ założycielski")]
        public FoundingBody FoundingBody
        {
            get => organZalozycielski;
            set => SetPropertyValue(nameof(FoundingBody), ref organZalozycielski, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Organ rejestrowy")]
        public RegistrationAuthority RegistrationAuthority
        {
            get => registrationAuthority;
            set => SetPropertyValue(nameof(RegistrationAuthority), ref registrationAuthority, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Rodzaj rejestru ewidencji")]
        public RegistryType RegistryType
        {
            get => registryType;
            set => SetPropertyValue(nameof(RegistryType), ref registryType, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        [XafDisplayName("Rodzaje działaności PKD")]
        [Association("Customer-CustomerPkdCodes"), DevExpress.Xpo.Aggregated]
        public XPCollection<CustomerPkd> PkdCodes
        {
            get
            {
                return GetCollection<CustomerPkd>(nameof(PkdCodes));
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [XafDisplayName("Numer w rejestrze ewidencji")]
        public string NumberInRegisterOfRecords
        {
            get => numberInRegisterOfRecords;
            set => SetPropertyValue(nameof(NumberInRegisterOfRecords), ref numberInRegisterOfRecords, value);
        }


        [XafDisplayName("Data powstania")]
        public DateTime DateOfCreation
        {
            get => dateOfCreation;
            set => SetPropertyValue(nameof(DateOfCreation), ref dateOfCreation, value);
        }


        [XafDisplayName("Data rozpoczęcia działalności")]
        public DateTime DateOfCommencementBusiness
        {
            get => dateOfCommencementBusiness;
            set => SetPropertyValue(nameof(DateOfCommencementBusiness), ref dateOfCommencementBusiness, value);
        }


        [XafDisplayName("Data wpisu do regon")]
        public DateTime DateOfEntryToRegon
        {
            get => dateOfEntryToRegon;
            set => SetPropertyValue(nameof(DateOfEntryToRegon), ref dateOfEntryToRegon, value);
        }


        [XafDisplayName("Data wpisu do rejestru ewidencji")]
        public DateTime DateOfEntryToRegisterOfRecords
        {
            get => dateOfEntryToRegisterOfRecords;
            set => SetPropertyValue(nameof(DateOfEntryToRegisterOfRecords), ref dateOfEntryToRegisterOfRecords, value);
        }


        [XafDisplayName("Data zaistnienia zmiany")]
        public DateTime DateOfChangeOccurrence
        {
            get => dateOfChangeOccurrence;
            set => SetPropertyValue(nameof(DateOfChangeOccurrence), ref dateOfChangeOccurrence, value);
        }


        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public StreetGus StreetGus
        {
            get => ulicaGus;
            set => SetPropertyValue(nameof(StreetGus), ref ulicaGus, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public CityGus CityGus
        {
            get => miastoGus;
            set => SetPropertyValue(nameof(CityGus), ref miastoGus, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public CityGus PostCityGus

        {
            get => miastoPocztyGus;
            set => SetPropertyValue(nameof(PostCityGus), ref miastoPocztyGus, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public CommuneGus CommuneGus
        {
            get => gminaGus;
            set => SetPropertyValue(nameof(CommuneGus), ref gminaGus, value);
        }
        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public CountyGus CountyGus
        {
            get => powiatGus;
            set => SetPropertyValue(nameof(CountyGus), ref powiatGus, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public ProvinceGus ProvinceGus
        {
            get => wojewodztwoGus;
            set => SetPropertyValue(nameof(ProvinceGus), ref wojewodztwoGus, value);
        }

        [DetailViewLayout("Dane GUS", LayoutGroupType.SimpleEditorsGroup, 200)]
        public CountryGus CountryGus
        {
            get => krajGus;
            set => SetPropertyValue(nameof(CountryGus), ref krajGus, value);
        }
        #endregion
    }

    public enum Segment
    { 
        Corporate= 2,
        Consumer = 7,
        [XafDisplayName("Home Office")]
        HomeOffice = 0,
        [XafDisplayName("Small Business")]
        SmallBusiness =9
    }
}
