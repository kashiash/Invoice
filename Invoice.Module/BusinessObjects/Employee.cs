using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace Invoice.Module.BusinessObjects
{
    [DefaultClassOptions]
    [MapInheritance(MapInheritanceType.ParentTable)]
    [DefaultProperty("FullName")]
    [ImageName("BO_User")]
    public class Employee : ApplicationUser
    {
        private string _LastName;
        private string _FirstName;
        private Department department;
        public Employee(Session session) : base(session)
        {
        }

        public string FirstName
        {
            get { return _FirstName; }
            set { SetPropertyValue(nameof(FirstName), ref _FirstName, value); }
        }

        public string LastName
        {
            get { return _LastName; }
            set { SetPropertyValue(nameof(LastName), ref _LastName, value); }
        }

        [PersistentAlias("concat(FirstName, ' ', LastName)")]
        public string FullName { get { return Convert.ToString(EvaluateAlias(nameof(FullName))); } }

        [Association]
        [RuleRequiredField]
        public Department Department
        {
            get { return department; }
            set { SetPropertyValue(nameof(Department), ref department, value); }
        }
        //[Association]
        //public XPCollection<EmployeeTask> Tasks
        //{
        //    get
        //    {
        //        return GetCollection<EmployeeTask>("Tasks");
        //    }
        //}
    }
}
