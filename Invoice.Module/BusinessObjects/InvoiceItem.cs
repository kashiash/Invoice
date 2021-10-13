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
}
