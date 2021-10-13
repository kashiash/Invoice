using DevExpress.ExpressApp.Model;
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

        [Association]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public decimal Netto
        {
            get => netto;
            set => SetPropertyValue(nameof(Netto), ref netto, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public decimal Vat
        {
            get => vat;
            set => SetPropertyValue(nameof(Vat), ref vat, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public decimal Brutto
        {
            get => brutto;
            set => SetPropertyValue(nameof(Brutto), ref brutto, value);
        }


        [Association, Aggregated]
        public XPCollection<InvoiceItem> Items
        {
            get
            {
                return GetCollection<InvoiceItem>(nameof(Items));
            }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        internal void RecalculateTotals(bool forceChangeEvents)
        {
            decimal oldNetto = Netto;
            decimal? oldVAT = Vat;
            decimal? oldBrutto = Brutto;


            decimal tmpNetto = 0m;
            decimal tmpVAT = 0m;
            decimal tmpBrutto = 0m;

            foreach (var rec in Items)
            {
                tmpNetto += rec.Netto;
                tmpVAT += rec.Vat;
                tmpBrutto += rec.Brutto;
            }
            Netto = tmpNetto;
            Vat = tmpVAT;
            Brutto = tmpBrutto;

            if (forceChangeEvents)
            {
                OnChanged(nameof(Netto), oldNetto, Netto);
                OnChanged(nameof(Vat), oldVAT, Vat);
                OnChanged(nameof(Brutto), oldBrutto, Brutto);
            }
        }
    }
}
