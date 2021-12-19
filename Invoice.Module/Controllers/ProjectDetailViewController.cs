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
    public class ProjectDetailViewController : ObjectViewController<DetailView, Project>
    {
        private readonly PopupWindowShowAction uploadFileAction;

        public ProjectDetailViewController()
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

            var projectOid = ViewCurrentObject == null ? Guid.Empty : ViewCurrentObject.Oid;
            var customerOid = ViewCurrentObject == null || ViewCurrentObject.Customer == null ? Guid.Empty : ViewCurrentObject.Customer.Oid;

            uploadFile.AssignedFileDataTemp.Customer = customerOid;
            uploadFile.AssignedFileDataTemp.Project = projectOid;
            e.View = Application.CreateDetailView(objectSpace, uploadFile);
        }

        private void UploadFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }
    }
}
