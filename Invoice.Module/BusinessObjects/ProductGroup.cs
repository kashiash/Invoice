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
    [NavigationItem("Others")]
    public class ProductGroup : XPObject
    {
        public ProductGroup(Session session) : base(session)
        { }


        string notes;
        string groupName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string GroupName
        {
            get => groupName;
            set => SetPropertyValue(nameof(GroupName), ref groupName, value);
        }

        [Association("Product-Products")]
        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        public XPCollection<Product> Products
        {
            get
            {
                return GetCollection<Product>(nameof(Products));
            }
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }

    }
}
