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
    public class ProductGroup : XPObject
    {
        public ProductGroup(Session session) : base(session)
        { }


        string groupName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string GroupName
        {
            get => groupName;
            set => SetPropertyValue(nameof(GroupName), ref groupName, value);
        }

        [Association("Product-Products")]
        public XPCollection<Product> Products
        {
            get
            {
                return GetCollection<Product>(nameof(Products));
            }
        }

    }
}
