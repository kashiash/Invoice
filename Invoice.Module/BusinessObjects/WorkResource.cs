using DevExpress.Xpo;
using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    public class WorkResource : XPObject
    {
        public WorkResource(Session session) : base(session)
        {
        }


        decimal unitPrice;
        string resourceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ResourceName
        {
            get { return resourceName; }
            set { SetPropertyValue(nameof(ResourceName), ref resourceName, value); }
        }


        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref unitPrice, value); }
        }
    }
}
