using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    [Appearance("PaymentIfBalanceZero", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "PaymentBalance = 0", Context = "ListView", FontColor = "Blue", Priority = 101)]
    public class Payment : CustomBaseObject
    {
        public Payment(Session session) : base(session)
        { }


  
        decimal paymentBalance;
        decimal sumOfPayments;
        string notes;
        string paymentDescription;
        Customer customer;
        decimal amount;
        DateTime paymentDate;

        public DateTime PaymentDate
        {
            get => paymentDate;
            set => SetPropertyValue(nameof(PaymentDate), ref paymentDate, value);
        }


        public decimal Amount
        {
            get => amount;
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }

        [Association("Customer-Payments")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }



        public decimal SumOfPayments
        {
            get => sumOfPayments;
            set => SetPropertyValue(nameof(SumOfPayments), ref sumOfPayments, value);
        }


        public decimal PaymentBalance
        {
            get => paymentBalance;
            set => SetPropertyValue(nameof(PaymentBalance), ref paymentBalance, value);
        }
        [Action(Caption = "Find invoices", TargetObjectsCriteria = "SumOfPayments < Amount", ImageName = "BO_Skull", AutoCommit = true)]
        public void FindInvoicesForPayment()
        {
            if (Customer != null)
            {
                var invoices = customer.Invoices
                    .Where(i => i.SumOfPayments < i.TotalBrutto)
                    .OrderBy(i => i.PaymentDate);

                foreach (var invoice in invoices)
                {
                    decimal rest = RegisterPayments2Invoice(invoice);

                    if (rest <= 0)
                    {
                        break;
                    }
                }
            }
        }



        [Association, Aggregated]
        [DetailViewLayoutAttribute("PaymentsAndNotes", LayoutGroupType.TabbedGroup, 100)]
        public XPCollection<InvoicePayment> InvoicePayments
        {
            get
            {
                return GetCollection<InvoicePayment>(nameof(InvoicePayments));
            }
        }

        [DetailViewLayoutAttribute("PaymentsAndNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string PaymentDescription
        {
            get => paymentDescription;
            set => SetPropertyValue(nameof(PaymentDescription), ref paymentDescription, value);
        }

        [DetailViewLayoutAttribute("PaymentsAndNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

        public void CalculateSumOfPayments(bool forceChangeEvents = true)
        {
            decimal? oldSumOfPayments = sumOfPayments;

            decimal sumOfPaymentsTotal = 0m;

            foreach (var payment in InvoicePayments)
            {
                sumOfPaymentsTotal += payment.Amount;
            }
            sumOfPayments = sumOfPaymentsTotal;
            paymentBalance = amount - sumOfPayments;

            if (forceChangeEvents)
            {
                OnChanged(nameof(SumOfPayments), oldSumOfPayments, sumOfPayments);
                OnChanged(nameof(PaymentBalance));
            }
        }

        public decimal RegisterPayments2Invoice(BusinessObjects.Invoice invoice)
        {
            var balance = Amount - SumOfPayments;
            if (balance > 0)
            {
                var payment = new InvoicePayment(Session);
                payment.Payment = this;
                payment.Invoice = invoice;
                var dueAmount = invoice.TotalBrutto - invoice.SumOfPayments;
                payment.Amount = balance > dueAmount ? dueAmount : balance;
                InvoicePayments.Add(payment);
                CalculateSumOfPayments(true);
                return balance - payment.Amount;
            }

            return 0;
        }
        protected override void OnSaving()
        {
            PaymentBalance = Amount - SumOfPayments;

        }
    }
}
