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
    public class Payment : XPObject
    {
        public Payment(Session session) : base(session)
        { }


        bool paymentsAssigned;
        decimal transactionBalance;
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


        public decimal TransactionBalance
        {
            get => transactionBalance;
            set => SetPropertyValue(nameof(TransactionBalance), ref transactionBalance, value);
        }

        public void FindInvoicesForPayment()
        {
                if (Customer != null )
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

        public bool PaymentsAssigned
        {
            get => paymentsAssigned;
            set => SetPropertyValue(nameof(PaymentsAssigned), ref paymentsAssigned, value);
        }


        [Association,Aggregated]
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

        public void CalculateSumOfPayments(bool forceChangeEvents)
        {
            decimal? oldSumOfPayments = sumOfPayments;

            decimal sumOfPaymentsTotal = 0m;

            foreach (var payment in InvoicePayments)
            {
                sumOfPaymentsTotal += payment.Amount;
            }
            sumOfPayments = sumOfPaymentsTotal;
            transactionBalance = amount - sumOfPayments;

            if (forceChangeEvents)
            {
                OnChanged(nameof(SumOfPayments), oldSumOfPayments, sumOfPayments);
                OnChanged(nameof(TransactionBalance));
            }
        }

        public decimal RegisterPayments2Invoice(BusinessObjects.Invoice invoice)
        {
            var balance = Amount - SumOfPayments;
            if (balance > 0)
            {
                var payment = new InvoicePayment(Session);
                payment.Payment = this;
                var dueAmount = invoice.TotalBrutto - invoice.SumOfPayments;
                payment.Amount = balance > dueAmount ? dueAmount : balance;
                InvoicePayments.Add(payment);
                CalculateSumOfPayments(true);
                return balance - payment.Amount;
            }

            return 0;
        }
    }
}
