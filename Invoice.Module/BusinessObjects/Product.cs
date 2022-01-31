using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class Product : CustomBaseObject
    {
        public Product(Session session) : base(session)
        {
        }

        string shortName;
        ProductCategory category;
        string notes;
        string gTIN;
        string productName;
        string symbol;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Symbol { get { return symbol; } set { SetPropertyValue(nameof(Symbol), ref symbol, value); } }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ProductName
        {
            get { return productName; }
            set { SetPropertyValue(nameof(ProductName), ref productName, value); }
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string ShortName
        {
            get { return shortName; }
            set { SetPropertyValue(nameof(ShortName), ref shortName, value); }
        }


        //   [Association("ProductCategory-Products")]
        [ImmediatePostData]
        public ProductCategory Category
        {
            get { return category; }
            set
            {
                var modified = SetPropertyValue(nameof(Category), ref category, value);
                if(modified && !IsLoading && !IsSaving)
                {
                    foreach(var attribute in ProductAssignedAttributeValues.ToList())
                    {
                        if(Category == null || Category.ProductCategoryAttributes.Count == 0)
                        {
                            ProductAssignedAttributeValues.Remove(attribute);
                            continue;
                        }
                        if(Category.ProductCategoryAttributes.Any(p => p.Attribute == attribute.ProductAttribute) ==
                            false)
                        {
                            ProductAssignedAttributeValues.Remove(attribute);
                        }
                    }
                    if(Category != null || Category.ProductCategoryAttributes.Count > 0)
                    {
                        foreach(var attribute in Category.ProductCategoryAttributes)
                        {
                            if(ProductAssignedAttributeValues.Any(p => p.ProductAttribute == attribute.Attribute) ==
                                false)
                            {
                                var aa = new ProductAssignedAttributeValue(Session);
                                aa.Product = this;
                                aa.ProductAttribute = attribute.Attribute;
                                aa.SelectedValue = attribute.Attribute.DefaultValue;
                                ProductAssignedAttributeValues.Add(aa);
                            }
                        }

                        if(Category.ForceRefreshProductSymbol)
                        {
                            SetSymbol();
                        }
                    }
                }
            }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string GTIN { get { return gTIN; } set { SetPropertyValue(nameof(GTIN), ref gTIN, value); } }


        VatRate vatRate;
        decimal unitPrice;

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref unitPrice, value); }
        }


        public VatRate VatRate
        {
            get { return vatRate; }
            set { SetPropertyValue(nameof(VatRate), ref vatRate, value); }
        }

        [DetailViewLayout("GroupsAndNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string Notes { get { return notes; } set { SetPropertyValue(nameof(Notes), ref notes, value); } }

        [Association("Product-Products")]
        [DetailViewLayout("GroupsAndNotes", LayoutGroupType.TabbedGroup, 100)]
        public XPCollection<ProductGroup> Groups { get { return GetCollection<ProductGroup>(nameof(Groups)); } }

        [Association("Product-ProductAssignedAttributeValues"), Aggregated]
        [DetailViewLayout("GroupsAndNotes", LayoutGroupType.TabbedGroup, 100)]
        public XPCollection<ProductAssignedAttributeValue> ProductAssignedAttributeValues
        {
            get { return GetCollection<ProductAssignedAttributeValue>(nameof(ProductAssignedAttributeValues)); }
        }

        public string AppliedGroups
        {
            get
            {
                if(Groups != null)
                {
                    return string.Join(", ", Groups.Select(g => g.GroupName).ToArray());
                } else
                {
                    return "N/A";
                }
            }
        }

        [Action(Caption =  "Set symbol", AutoCommit = true)]
        public void SetSymbol()
        {
            if(ProductAssignedAttributeValues != null && ProductAssignedAttributeValues.Count > 0)
            {
                Symbol = $"{ShortName} {string.Join(", ", ProductAssignedAttributeValues.Where(g => g.SelectedValue != null && string.IsNullOrEmpty(g.SelectedValue.Value) == false).Select(g => g.SelectedValue.Value).ToArray())}";
            }
        }
    }
}
