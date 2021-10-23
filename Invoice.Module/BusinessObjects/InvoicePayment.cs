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
            set => SetPropertyValue(nameof(Amount), ref amount, value);
        }


        [Association]
        [RuleRequiredField]
        public Invoice Invoice
        {
            get => invoice;
            set => SetPropertyValue(nameof(Invoice), ref invoice, value);
        }

        [Association]
        [RuleRequiredField]
        public Payment Payment
        {
            get => payment;
            set => SetPropertyValue(nameof(Payment), ref payment, value);
        }

    }
}
