using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    public class ProductAssignedAttributeValue : XPObject
    {
        public ProductAssignedAttributeValue(Session session) : base(session)
        { }



        ProductAttributeAvaiableValue selectedValue;
        Product product;
        ProductAttribute productAttribute;

        [ImmediatePostData]
        public ProductAttribute ProductAttribute
        {
            get => productAttribute;
            set => SetPropertyValue(nameof(ProductAttribute), ref productAttribute, value);
        }


        [DataSourceProperty("ProductAttribute.Values")]
        public ProductAttributeAvaiableValue SelectedValue
        {
            get => selectedValue;
            set => SetPropertyValue(nameof(SelectedValue), ref selectedValue, value);
        }


        [Association("Product-ProductAssignedAttributeValues")]
        public Product Product
        {
            get => product;
            set => SetPropertyValue(nameof(Product), ref product, value);
        }
    }
}
