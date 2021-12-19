using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects
{
    [NavigationItem("Planning")]
    public class Project : BaseObject
    {
        public Project(Session session) : base(session) { }
        Customer customer;
        string name;
        public string Name
        {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }

        
        [Association("Customer-Projects")]
        public Customer Customer
        {
            get => customer;
            set => SetPropertyValue(nameof(Customer), ref customer, value);
        }

        PermissionPolicyUser manager;
        public PermissionPolicyUser Manager
        {
            get { return manager; }
            set { SetPropertyValue(nameof(Manager), ref manager, value); }
        }
        string description;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get { return description; }
            set { SetPropertyValue(nameof(Description), ref description, value); }
        }
        [Association, Aggregated]
        public XPCollection<ProjectTask> Tasks
        {
            get { return GetCollection<ProjectTask>(nameof(Tasks)); }
        }

        [Association("Project-AssignedFileData")]
        public XPCollection<AssignedFileData> AssignedFileData
        {
            get
            {
                return GetCollection<AssignedFileData>(nameof(AssignedFileData));
            }
        }
    }
}
