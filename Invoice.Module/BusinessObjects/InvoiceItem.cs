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

        [ImmediatePostData]
        public Product Product
        {
            get => product;
            set
            {
                bool modified = SetPropertyValue(nameof(Product), ref product, value);
                if (modified && !IsLoading && !IsSaving && Product != null)
                {
                    unitPrice = Product.UnitPrice;
                    vatRate = Product.VatRate;
                    RecalculateItem();

                }
            }
        }


        [Association]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        [ImmediatePostData]
        public decimal Quantity
        {
            get => quantity;
            set
            {
                bool modified = SetPropertyValue(nameof(Quantity), ref quantity, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    RecalculateItem();
                }
            }
        }

        [ImmediatePostData]
        public decimal UnitPrice
        {
            get => unitPrice;
            set
            {
                bool modified = SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    RecalculateItem();
                }
            }
        }

        VatRate vatRate;
        [ImmediatePostData]
        public VatRate VatRate
        {
            get => vatRate;
            set
            {
                bool modified = SetPropertyValue(nameof(VatRate), ref vatRate, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    RecalculateItem();
                }
            }
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

        private void RecalculateItem()
        {


           Netto = Quantity * UnitPrice;
            if (Product != null && Product.VatRate != null)
            {
                Brutto = Netto * (100 + Product.VatRate.Value) / 100;
            }
            else
            {
                Brutto = Netto;

            }
           Vat = Brutto - Netto;

            if (Invoice != null)
            {
                Invoice.RecalculateTotals(true);
            }
        }

    }
}
