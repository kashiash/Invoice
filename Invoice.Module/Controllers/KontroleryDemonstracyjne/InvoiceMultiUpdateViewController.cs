using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers.KontroleryDemonstracyjne
{
    public class InvoiceMultiUpdateViewController : ObjectViewController<ListView, BusinessObjects.Invoice>
    {
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction MultiUpdate;
        public InvoiceMultiUpdateViewController()
        {
            MultiUpdate = new PopupWindowShowAction(this, "IKosztyMultiUpdate", PredefinedCategory.Unspecified);
            MultiUpdate.Caption = "Aktualizuj zaznaczone rekordy";
            MultiUpdate.ImageName = "Update";
            
            MultiUpdate.TargetViewNesting = Nesting.Any;
            MultiUpdate.TargetViewType = ViewType.Any;
            MultiUpdate.TargetObjectType = typeof(BusinessObjects.Invoice); // To jest nadmiarowe bo jest podane w  ObjectViewController<ListView, BusinessObjects.Invoice>
            MultiUpdate.ToolTip = "Modyfikuj zaznaczone rekordy";
            MultiUpdate.TypeOfView = typeof(ListView);  // To jest nadmiarowe bo jest podane w  ObjectViewController<ListView, BusinessObjects.Invoice>
            MultiUpdate.CustomizePopupWindowParams += MultiUpdate_CustomizePopupWindowParams;
            MultiUpdate.Execute += MultiUpdate_Execute;
        }

        private void MultiUpdate_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            e.View = Application.CreateDetailView(Application.CreateObjectSpace(), new InvoiceUpdateParameters());
        }


        private void MultiUpdate_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var parameters = (InvoiceUpdateParameters)e.PopupWindow.View.CurrentObject;
            foreach (var obj in e.SelectedObjects) //Cast<BusinessObjects.Invoice>().ToList()
            {
                if (obj is BusinessObjects.Invoice invoice)
                {
                    if (!string.IsNullOrEmpty(parameters.Notes))
                    {
                        if (parameters.UpdateType == NotesUpdateType.Append)
                        {
                            invoice.Notes = $"{invoice.Notes} {parameters.Notes}";
                        }
                        else
                        {
                            invoice.Notes = parameters.Notes;
                        }
                    }
                    if (parameters.PaymentDate > DateTime.MinValue)
                    {
                        invoice.PaymentDate = parameters.PaymentDate;
                    }
                }
            }
            View.ObjectSpace.CommitChanges();
            View.Refresh();
        }
    }
    [NonPersistent]
    [XafDisplayName("Update invoice")]
    public class InvoiceUpdateParameters
    {
        [Size(SizeAttribute.Unlimited)]
        public string Notes { get; set; }
        public NotesUpdateType UpdateType { get; set; }
        public DateTime PaymentDate { get; set; }

    }

    public enum NotesUpdateType
    {
        Append = 0,
        Replace = 1,
    }
}
