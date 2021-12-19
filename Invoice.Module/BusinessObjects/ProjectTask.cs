using DevExpress.ExpressApp.Model;
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
    public class ProjectTask : BaseObject
    {
        public ProjectTask(Session session) : base(session) { }
        string subject;
        [Size(255)]
        public string Subject
        {
            get { return subject; }
            set { SetPropertyValue(nameof(Subject), ref subject, value); }
        }
        ProjectTaskStatus status;
        public ProjectTaskStatus Status
        {
            get { return status; }
            set { SetPropertyValue(nameof(Status), ref status, value); }
        }
        PermissionPolicyUser assignedTo;
        public PermissionPolicyUser AssignedTo
        {
            get { return assignedTo; }
            set { SetPropertyValue(nameof(AssignedTo), ref assignedTo, value); }
        }
        DateTime startDate;
        [ModelDefault("DisplayFormat", "{0:g}")]
        [ModelDefault("EditMask", "g")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetPropertyValue(nameof(startDate), ref startDate, value); }
        }
        DateTime endDate;
        [ModelDefault("DisplayFormat", "{0:g}")]
        [ModelDefault("EditMask", "g")]
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetPropertyValue(nameof(endDate), ref endDate, value); }
        }
        string notes;
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get { return notes; }
            set { SetPropertyValue(nameof(Notes), ref notes, value); }
        }


        [Association("ProjectTask-AssignedFileData")]
        public XPCollection<AssignedFileData> AssignedFileData
        {
            get
            {
                return GetCollection<AssignedFileData>(nameof(AssignedFileData));
            }
        }

        Project project;
        [Association]
        public Project Project
        {
            get { return project; }
            set
            {
                bool modified = SetPropertyValue(nameof(Project), ref project, value);
                if (!IsSaving && !IsLoading && modified && project != null)
                {
                    AssignedTo = Project.Manager;
                }
            }
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            StartDate = DateTime.Now;
        }
    }


    public enum ProjectTaskStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Completed = 2,
        Deferred = 3
    }
}
