using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;


namespace Invoice.Module.BusinessObjects
{
    public class AssignedFileData : FileAttachmentBase
    {
        public AssignedFileData(Session session) : base(session) { }
        string notes;
        string description;
        ProjectTask task;
        Project project;
        private Customer customer;
        [Association("Customer-AssignedFileData")]
        public Customer Customer
        {
            get { return customer; }
            set { SetPropertyValue(nameof(BusinessObjects.Customer), ref customer, value); }
        }


        [Association("Project-AssignedFileData")]
        public Project Project
        {
            get => project;
            set
            {

                var modified = SetPropertyValue(nameof(Project), ref project, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    Customer = Project?.Customer;
                }
            }
        }


        [Association("ProjectTask-AssignedFileData")]
        public ProjectTask Task
        {
            get => task;
            set
            {
                var modified = SetPropertyValue(nameof(Task), ref task, value);
                if (modified && !IsLoading && !IsSaving)
                {
                    Project = Task?.Project;
                }
            }
        }


        [Size(200)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }

        
        [Size(SizeAttribute.Unlimited)]
        public string Notes
        {
            get => notes;
            set => SetPropertyValue(nameof(Notes), ref notes, value);
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();
            documentType = DocumentType.Unknown;
        }
        private DocumentType documentType;
        public DocumentType DocumentType
        {
            get { return documentType; }
            set { SetPropertyValue(nameof(DocumentType), ref documentType, value); }
        }
    }
    public enum DocumentType
    {
        Invoice = 1, Photos = 2, Documentation = 3,
        Diagrams = 4, ScreenShots = 5, Unknown = 6
    };
}
