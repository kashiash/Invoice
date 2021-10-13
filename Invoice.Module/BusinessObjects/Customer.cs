using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Customer : BaseObject
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


        [Association]
        public XPCollection<InvoiceItem> Items
        {
            get
            {
                return GetCollection<InvoiceItem>(nameof(Items));
            }
        }



    }
}
