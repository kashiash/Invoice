using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using Invoice.Module.BusinessObjects;
using Invoice.Module.BusinessObjects.NonPersistent;
using Invoice.Module.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers
{
    public class CustomerDetailViewController : ObjectViewController<DetailView, Customer>
    {
        private readonly SimpleAction getDataFromGusAction;
        private readonly PopupWindowShowAction uploadFileAction;

        public CustomerDetailViewController()
        {
            getDataFromGusAction = new SimpleAction(this, $"{GetType().FullName}{nameof(getDataFromGusAction)}", PredefinedCategory.Unspecified)
            {
                Caption = "Pobierz dane z GUS",
                ImageName = "Szukaj",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
            };
            getDataFromGusAction.Execute += GetDataFromGusAction_Execute;

            uploadFileAction = new PopupWindowShowAction(this, $"{GetType().FullName}{nameof(uploadFileAction)}", PredefinedCategory.Unspecified)
            {
                Caption = "Upload files",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage
            };
            uploadFileAction.CustomizePopupWindowParams += UploadFileAction_CustomizePopupWindowParams;
            uploadFileAction.Execute += UploadFileAction_Execute;
        }

        private void GetDataFromGusAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (ViewCurrentObject == null || string.IsNullOrWhiteSpace(ViewCurrentObject.VatNumber))
            {
                return;
            }

            var gusService = new GusService(ObjectSpace);
            gusService.GetDataFromGus(ViewCurrentObject);
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }

        private void UploadFileAction_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var objectSpace = Application.CreateObjectSpace(typeof(UploadAssignedFileData));
            var uploadFile = objectSpace.CreateObject<UploadAssignedFileData>();

            var customerOid = ViewCurrentObject == null ? Guid.Empty : ViewCurrentObject.Oid;

            uploadFile.AssignedFileDataTemp.Customer = customerOid;
            e.View = Application.CreateDetailView(objectSpace, uploadFile);
        }

        private void UploadFileAction_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            ObjectSpace.CommitChanges();
            ObjectSpace.Refresh();
        }
    }
}
