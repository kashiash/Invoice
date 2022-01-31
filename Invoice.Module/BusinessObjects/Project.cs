using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [NavigationItem("Planning")]
    public class Project : BaseObject
    {
        public Project(Session session) : base(session)
        {
        }
        Customer customer;
        string name;

        public string Name { get { return name; } set { SetPropertyValue(nameof(Name), ref name, value); } }


        [Association("Customer-Projects")]
        public Customer Customer
        {
            get { return customer; }
            set { SetPropertyValue(nameof(Customer), ref customer, value); }
        }

        Employee manager;

        public Employee Manager
        {
            get { return manager; }
            set { SetPropertyValue(nameof(Manager), ref manager, value); }
        }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association, Aggregated]
        public XPCollection<ProjectTask> Tasks { get { return GetCollection<ProjectTask>(nameof(Tasks)); } }

        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Association("Project-AssignedFileData")]
        public XPCollection<AssignedFileData> AssignedFileData
        {
            get { return GetCollection<AssignedFileData>(nameof(AssignedFileData)); }
        }

        string description;
        [DetailViewLayoutAttribute("ItemsNotes", LayoutGroupType.TabbedGroup, 100)]
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return description; }
            set { SetPropertyValue(nameof(Description), ref description, value); }
        }
    }
}
