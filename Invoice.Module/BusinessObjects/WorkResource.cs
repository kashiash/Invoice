using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    public class WorkResource : XPObject
    {
        public WorkResource(Session session) : base(session)
        { }


        string resourceName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ResourceName
        {
            get => resourceName;
            set => SetPropertyValue(nameof(ResourceName), ref resourceName, value);
        }

    }
}
