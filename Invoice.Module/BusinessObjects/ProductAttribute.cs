using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Others")]
    public class ProductAttribute : XPObject
    {
        public ProductAttribute(Session session) : base(session)
        {
        }


        ProductAttributeAvaiableValue defaultValue;
        string notes;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name { get { return name; } set { SetPropertyValue(nameof(Name), ref name, value); } }


        [DataSourceProperty(nameof(Values))]
        public ProductAttributeAvaiableValue DefaultValue
        {
            get { return defaultValue; }
            set { SetPropertyValue(nameof(DefaultValue), ref defaultValue, value); }
        }


        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("ProductAttribute-Values")]
        public XPCollection<ProductAttributeAvaiableValue> Values
        {
            get { return GetCollection<ProductAttributeAvaiableValue>(nameof(Values)); }
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("ProductAttribute-ProductCategoryAttributes")]

        public XPCollection<ProductCategoryAttribute> Categories
        {
            get { return GetCollection<ProductCategoryAttribute>(nameof(Categories)); }
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string Notes { get { return notes; } set { SetPropertyValue(nameof(Notes), ref notes, value); } }
    }
}
