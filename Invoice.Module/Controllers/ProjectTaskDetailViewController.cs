using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using Invoice.Module.BusinessObjects;
using Invoice.Module.BusinessObjects.NonPersistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers
{
    public class ProjectTaskDetailViewController : ObjectViewController<DetailView, ProjectTask>
    {
        private readonly PopupWindowShowAction uploadFileAction;

        public ProjectTaskDetailViewController()
        {
            uploadFileAction = new PopupWindowShowAction(this, $"{GetType().FullName}{nameof(uploadFileAction)}", PredefinedCategory.Unspecified)
            {
                Caption = "Upload files",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage
            };
            uploadFileAction.CustomizePopupWindowParams += UploadFileAction_CustomizePopupWindowParams;
            uploadFileAction.Execute += UploadFileAction_Execute;
        }

        private void UploadFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var objectSpace = Application.CreateObjectSpace(typeof(UploadAssignedFileData));
            var uploadFile = objectSpace.CreateObject<UploadAssignedFileData>();

            var taskOid = ViewCurrentObject == null ? Guid.Empty : ViewCurrentObject.Oid;
            var projectOid = ViewCurrentObject == null || ViewCurrentObject.Project == null ? Guid.Empty : ViewCurrentObject.Project.Oid;
            var customerOid = ViewCurrentObject == null || ViewCurrentObject.Project == null || ViewCurrentObject.Project.Customer == null
                ? Guid.Empty : ViewCurrentObject.Project.Customer.Oid;

            uploadFile.AssignedFileDataTemp.Customer = customerOid;
            uploadFile.AssignedFileDataTemp.Project = projectOid;
            uploadFile.AssignedFileDataTemp.Task = taskOid;
            e.View = Application.CreateDetailView(objectSpace, uploadFile);
        }

        private void UploadFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }
    }
}
