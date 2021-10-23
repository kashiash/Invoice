using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(InvoiceNumber))]
    public class Invoice : BaseObject
    {
        public Invoice(Session session) : base(session)
        { }


        DateTime paymentDate;
        decimal sumOfPayments;
        string notes;
        decimal totalBrutto;
        decimal totalVat;
        decimal totalNetto;
        Customer customer;
        DateTime dueDate;
        DateTime invoiceDate;
        string invoiceNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [RuleRequiredField]
        [RuleUniqueValue]
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



        public decimal SumOfPayments
        {
            get => sumOfPayments;
            set => SetPropertyValue(nameof(SumOfPayments), ref sumOfPayments, value);
        }


        public DateTime PaymentDate
        {
            get => paymentDate;
            set => SetPropertyValue(nameof(PaymentDate), ref paymentDate, value);
        }

        [Association]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public decimal TotalNetto
        {
            get => totalNetto;
            set => SetPropertyValue(nameof(TotalNetto), ref totalNetto, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public decimal TotalVat
        {
            get => totalVat;
            set => SetPropertyValue(nameof(TotalVat), ref totalVat, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public decimal TotalBrutto
        {
            get => totalBrutto;
            set => SetPropertyValue(nameof(TotalBrutto), ref totalBrutto, value);
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<InvoiceItem> InvoiceItems
        {
            get
            {
                return GetCollection<InvoiceItem>(nameof(InvoiceItems));
            }
        }
        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association, DevExpress.Xpo.Aggregated]
        public XPCollection<InvoicePayment> Payments
        {
            get
            {
                return GetCollection<InvoicePayment>(nameof(Payments));
            }
        }

        [Size(SizeAttribute.Unlimited)]
        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        internal void RecalculateTotals(bool forceChangeEvents)
        {
            decimal oldNetto = TotalNetto;
            decimal? oldVAT = TotalVat;
            decimal? oldBrutto = TotalBrutto;


            decimal tmpNetto = 0m;
            decimal tmpVAT = 0m;
            decimal tmpBrutto = 0m;

            foreach (var rec in InvoiceItems)
            {
                tmpNetto += rec.Netto;
                tmpVAT += rec.Vat;
                tmpBrutto += rec.Brutto;
            }
            TotalNetto = tmpNetto;
            TotalVat = tmpVAT;
            TotalBrutto = tmpBrutto;

            if (forceChangeEvents)
            {
                OnChanged(nameof(TotalNetto), oldNetto, TotalNetto);
                OnChanged(nameof(TotalVat), oldVAT, TotalVat);
                OnChanged(nameof(TotalBrutto), oldBrutto, TotalBrutto);
            }
        }

        public void CalculateSumOfPayments(bool forceChangeEvents)
        {
            decimal? oldSumOfPayments = sumOfPayments;

            decimal tempSumOfPayemnts = 0m;
            paymentDate = DateTime.MinValue;
            foreach (var payment in Payments.OrderBy(w => w.Payment.PaymentDate))
            {
                tempSumOfPayemnts += payment.Amount;
                if (paymentDate != payment.Payment.PaymentDate && tempSumOfPayemnts >= TotalBrutto)
                {
                    paymentDate = payment.Payment.PaymentDate;
                }
            }

            sumOfPayments = tempSumOfPayemnts;

            if (forceChangeEvents)
            {
                OnChanged(nameof(SumOfPayments), oldSumOfPayments, sumOfPayments);
            }
        }
    }
}
