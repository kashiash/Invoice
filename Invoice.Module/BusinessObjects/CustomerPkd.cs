using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using GUS.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    public class CustomerPkd : BaseObject
    {
        public CustomerPkd(Session session) : base(session)
        { }


        PkdCode pkdCode;
        Customer customer;

        [Association("Customer-CustomerPkdCodes")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }


        public PkdCode PkdCode
        {
            get => pkdCode;
            set => SetPropertyValue(nameof(PkdCode), ref pkdCode, value);
        }
    }
}
