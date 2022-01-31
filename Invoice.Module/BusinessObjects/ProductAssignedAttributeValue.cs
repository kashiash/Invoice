using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    public class ProductAssignedAttributeValue : XPObject
    {
        public ProductAssignedAttributeValue(Session session) : base(session)
        {
        }


        ProductAttributeAvaiableValue selectedValue;
        Product product;
        ProductAttribute productAttribute;

        [ImmediatePostData]
        public ProductAttribute ProductAttribute
        {
            get { return productAttribute; }
            set { SetPropertyValue(nameof(ProductAttribute), ref productAttribute, value); }
        }


        [DataSourceProperty("ProductAttribute.Values")]
        public ProductAttributeAvaiableValue SelectedValue
        {
            get { return selectedValue; }
            set { SetPropertyValue(nameof(SelectedValue), ref selectedValue, value); }
        }


        [Association("Product-ProductAssignedAttributeValues")]
        public Product Product
        {
            get { return product; }
            set { SetPropertyValue(nameof(Product), ref product, value); }
        }
    }
}
