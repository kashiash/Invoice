using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty(nameof(InvoiceNumber))]

    [Appearance("InvoiceIfPayed", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "SumOfPayments >= TotalBrutto", Context = "ListView", FontColor = "Blue", Priority = 101)]

    [Appearance("InvoiceIfOverDue", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "OverDue = TRue", Context = "ListView", FontColor = "Red", Priority = 101)]

    public class Invoice : CustomBaseObject
    {
        public Invoice(Session session) : base(session)
        { }

        //[Browsable(false)]
        public bool OverDue => SumOfPayments < TotalBrutto && PaymentDate < DateTime.Now;
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

        internal void RecalculateTotals(bool forceChangeEvents = true)
        {
            decimal oldNetto = TotalNetto;
            decimal oldVAT = TotalVat;
            decimal oldBrutto = TotalBrutto;


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

        public void CalculateSumOfPayments(bool forceChangeEvents = true)
        {
            decimal? oldSumOfPayments = sumOfPayments;

            decimal tempSumOfPayemnts = 0m;
            paymentDate = DateTime.MinValue;
            foreach (var payment in Payments.OrderBy(w => w.Payment?.PaymentDate))
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

        [Action(Caption = "Find payments",TargetObjectsCriteria = "SumOfPayments < TotalBrutto", ImageName = "BO_Skull",AutoCommit =true)]
        public void FindPaymentsForInvoice()
        {
            if (Customer != null)
            {
                var payments = customer.Payments
                    .Where(i => i.SumOfPayments < i.Amount)
                    .OrderBy(i => i.PaymentDate);

                foreach (var payment in payments)
                {
                    _ = payment.RegisterPayments2Invoice(this);
                }
            }
        }
    }
}
