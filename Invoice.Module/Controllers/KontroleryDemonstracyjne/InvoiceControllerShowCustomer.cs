using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers.KontroleryDemonstracyjne
{
    public class InvoiceControllerShowCustomer : ObjectViewController<ListView, BusinessObjects.Invoice>
    {
        SimpleAction showCustomerDetailView;
        public InvoiceControllerShowCustomer()
        {
            showCustomerDetailView = new SimpleAction(this, $"{GetType().FullName}.{nameof(showCustomerDetailView)}", "View")
            {
                Caption = "Show Customer",
                ImageName = "Meeting",
                SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject,
                ActionMeaning = DevExpress.ExpressApp.Actions.ActionMeaning.Accept,

        };
            showCustomerDetailView.Execute += showCustomerDetailView_Execute;
            
        }
        private void showCustomerDetailView_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            IObjectSpace os = Application.CreateObjectSpace();
            var current = (BusinessObjects.Invoice)View.CurrentObject;
            var objectToDisplay = os.GetObject(current.Customer);

            string detailId = Application.FindDetailViewId(objectToDisplay.GetType());
            DetailView detailView = Application.CreateDetailView(os, detailId, true, objectToDisplay);
            detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
            e.ShowViewParameters.CreatedView = detailView;
            e.ShowViewParameters.Context = TemplateContext.View;
            e.ShowViewParameters.TargetWindow = TargetWindow.Default;
        }

    }
}
