using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    public class InvoicePayment : XPObject
    {
        public InvoicePayment(Session session) : base(session)
        { }


        Payment payment;
        Invoice invoice;
        decimal amount;

        public decimal Amount
        {
            get => amount;
            set
            {
             var modified =   SetPropertyValue(nameof(Amount), ref amount, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    invoice?.CalculateSumOfPayments();
                    payment?.CalculateSumOfPayments();
                }
            }
        }


        [Association]
        [RuleRequiredField]
        public Invoice Invoice
        {
            get => invoice;
            set
            {
                var oldInvoice = invoice;
                var modified = SetPropertyValue(nameof(Invoice), ref invoice, value);
                if (modified && !IsSaving && !IsLoading)
                {
                    invoice?.CalculateSumOfPayments();
                    oldInvoice?.CalculateSumOfPayments();
                }
            }
        }

        [Association]
        [RuleRequiredField]
        public Payment Payment
        {
            get => payment;
            set
            {
                var oldPayment = payment;
               var modified = SetPropertyValue(nameof(Payment), ref payment, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    payment?.CalculateSumOfPayments();
                    oldPayment?.CalculateSumOfPayments();
                }
            }
        }

    }
}
