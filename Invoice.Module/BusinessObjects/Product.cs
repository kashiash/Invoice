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


        VatRate vatRate;
        decimal unitPrice;

        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }


        public VatRate VatRate
        {
            get => vatRate;
            set => SetPropertyValue(nameof(VatRate), ref vatRate, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        [Association("Product-Products")]
        public XPCollection<ProductGroup> Groups
        {
            get
            {
                return GetCollection<ProductGroup>(nameof(Groups));
            }
        }

        public string AppliedGroups
        {
            get
            {
                if (Groups != null)
                {
                    return string.Join(", ", Groups.Select(g => g.GroupName).ToArray());
                }
                else
                {
                    return "N/A";
                }
            }
        }
    }
}
