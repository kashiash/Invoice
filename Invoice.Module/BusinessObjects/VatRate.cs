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
    [NavigationItem("Others")]
    public class VatRate : XPLiteObject
    {
        public VatRate(Session session) : base(session)
        { }


        decimal rateValue;
        string symbol;

        [Size(3)]
        [Key(false)]
        public string Symbol
        {
            get => symbol;
            set => SetPropertyValue(nameof(Symbol), ref symbol, value);
        }

        
        public decimal Value
        {
            get => rateValue;
            set => SetPropertyValue(nameof(Value), ref rateValue, value);
        }
    }
}
