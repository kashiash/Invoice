using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    public class ProductCategoryAttribute : XPObject
    {
        public ProductCategoryAttribute(Session session) : base(session)
        { }


        bool required;
        string notes;
        ProductAttribute attribute;
        ProductCategory product;

        [Association("ProductCategory-ProductCategoryAttributes")]
        public ProductCategory Category
        {
            get => product;
            set => SetPropertyValue(nameof(Category), ref product, value);
        }


        [Association("ProductAttribute-ProductCategoryAttributes")]
        public ProductAttribute Attribute
        {
            get => attribute;
            set => SetPropertyValue(nameof(Attribute), ref attribute, value);
        }

        
        public bool Required
        {
            get => required;
            set => SetPropertyValue(nameof(Required), ref required, value);
        }


        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
    }
}
