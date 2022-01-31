using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [XafDefaultProperty(nameof(Value))]
    public class ProductAttributeAvaiableValue : XPObject
    {
        public ProductAttributeAvaiableValue(Session session) : base(session)
        {
        }


        ProductAttribute productAttribute;
        string _value;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Value { get { return _value; } set { SetPropertyValue(nameof(Value), ref _value, value); } }

        [Association("ProductAttribute-Values")]
        public ProductAttribute ProductAttribute
        {
            get { return productAttribute; }
            set { SetPropertyValue(nameof(ProductAttribute), ref productAttribute, value); }
        }
    }
}
