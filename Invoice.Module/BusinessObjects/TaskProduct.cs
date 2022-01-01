using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    public class TaskProduct : BaseObject
    {
        public TaskProduct(Session session) : base(session)
        { }


   
        string unit;
        string notes;
        decimal total;
        decimal quantity;
        Product product;
        ProjectTask projectTask;

        [Association("ProjectTask-TaskProducts")]
        public ProjectTask ProjectTask
        {
            get => projectTask;
            set => SetPropertyValue(nameof(ProjectTask), ref projectTask, value);
        }
        [ImmediatePostData]
        public Product Product
        {
            get => product;
            set
            {
                var modified = SetPropertyValue(nameof(Product), ref product, value);
                if (modified && !IsLoading && !IsSaving && Product != null)
                {
                    unitPrice = Product.UnitPrice;

                    RecalculateItem();
                }
            }
        }



        private void RecalculateItem()
        {
            Total = Quantity * UnitPrice;
        }

        [ImmediatePostData]
        public decimal Quantity
        {
            get => quantity;
            set
            {

                var modified = SetPropertyValue(nameof(Quantity), ref quantity, value);
                if (modified && !IsLoading && !IsSaving )
                {
                    RecalculateItem();
                }
            }
        }

        decimal unitPrice;

        public decimal UnitPrice
        {
            get => unitPrice;
            set => SetPropertyValue(nameof(UnitPrice), ref unitPrice, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Unit
        {
            get => unit;
            set => SetPropertyValue(nameof(Unit), ref unit, value);
        }

        public decimal Total

        {
            get => total;
            set => SetPropertyValue(nameof(Total), ref total, value);
        }

        
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }
    }
}
