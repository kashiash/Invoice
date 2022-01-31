using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Others")]
    public class ProductCategory : XPObject
    {
        public ProductCategory(Session session) : base(session)
        {
        }


        bool forceRefreshProductSymbol;
        string notes;
        string categoryName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CategoryName
        {
            get { return categoryName; }
            set { SetPropertyValue(nameof(CategoryName), ref categoryName, value); }
        }


        public bool ForceRefreshProductSymbol
        {
            get { return forceRefreshProductSymbol; }
            set { SetPropertyValue(nameof(ForceRefreshProductSymbol), ref forceRefreshProductSymbol, value); }
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("ProductCategory-ProductCategoryAttributes"),Aggregated]
        public XPCollection<ProductCategoryAttribute> ProductCategoryAttributes
        {
            get { return GetCollection<ProductCategoryAttribute>(nameof(ProductCategoryAttributes)); }
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string Notes { get { return notes; } set { SetPropertyValue(nameof(Notes), ref notes, value); } }
    }
}
