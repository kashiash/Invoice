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
    public class ProductCategory : XPObject
    {
        public ProductCategory(Session session) : base(session)
        { }


        bool forceRefreshProductSymbol;
        string notes;
        string categoryName;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string CategoryName
        {
            get => categoryName;
            set => SetPropertyValue(nameof(CategoryName), ref categoryName, value);
        }

        
        public bool ForceRefreshProductSymbol
        {
            get => forceRefreshProductSymbol;
            set => SetPropertyValue(nameof(ForceRefreshProductSymbol), ref forceRefreshProductSymbol, value);
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("ProductCategory-ProductCategoryAttributes"),Aggregated]
        public XPCollection<ProductCategoryAttribute> ProductCategoryAttributes
        {
            get
            {
                return GetCollection<ProductCategoryAttribute>(nameof(ProductCategoryAttributes));
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
