using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    public class TaskResource : XPObject
    {
        public TaskResource(Session session) : base(session)
        {
        }


        string unit;
        string notes;
        decimal total;
        decimal quantity;
        WorkResource resource;
        ProjectTask projectTask;

        [Association("ProjectTask-TaskResources")]
        public ProjectTask ProjectTask
        {
            get { return projectTask; }
            set { SetPropertyValue(nameof(ProjectTask), ref projectTask, value); }
        }

        [ImmediatePostData]
        public WorkResource Resource
        {
            get { return resource; }
            set
            {
                var modified = SetPropertyValue(nameof(Resource), ref resource, value);
                if(modified && !IsLoading && !IsSaving && Resource != null)
                {
                    unitPrice = Resource.UnitPrice;

                    RecalculateItem();
                }
            }
        }


        private void RecalculateItem() { Total = Quantity * UnitPrice; }

        [ImmediatePostData]
        public decimal Quantity
        {
            get { return quantity; }
            set
            {
                var modified = SetPropertyValue(nameof(Quantity), ref quantity, value);
                if(modified && !IsLoading && !IsSaving)
                {
                    RecalculateItem();
                }
            }
        }

        decimal unitPrice;

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref unitPrice, value); }
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Unit { get { return unit; } set { SetPropertyValue(nameof(Unit), ref unit, value); } }

        public decimal Total { get { return total; } set { SetPropertyValue(nameof(Total), ref total, value); } }


        [Size(SizeAttribute.Unlimited)]
        public string Notes { get { return notes; } set { SetPropertyValue(nameof(Notes), ref notes, value); } }
    }
}
