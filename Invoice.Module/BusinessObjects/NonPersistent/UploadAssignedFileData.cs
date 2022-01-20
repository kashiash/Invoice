using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.BusinessObjects.NonPersistent
{
    [DomainComponent]
    [DefaultClassOptions]
    public class UploadAssignedFileData : NonPersistentBaseObject
    {
        
        public UploadAssignedFileData()
        {
            AssignedFileDataTemp = new AssignedFileDataTemp();
        }

        [EditorAlias("UploadAssignedFileEditor")]
        public AssignedFileDataTemp AssignedFileDataTemp;
    }

    public class AssignedFileDataTemp : NonPersistentBaseObject
    {
        public Guid Task;
        public Guid Project;
        public Guid Customer;
    }
}
