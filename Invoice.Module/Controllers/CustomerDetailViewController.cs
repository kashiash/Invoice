using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using Invoice.Module.BusinessObjects;
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

        public CustomerDetailViewController()
        {
            getDataFromGusAction = new SimpleAction(this, $"{GetType().FullName}{nameof(getDataFromGusAction)}", PredefinedCategory.Unspecified)
            {
                Caption = "Pobierz dane z GUS",
                ImageName = "Szukaj",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
            };
            getDataFromGusAction.Execute += GetDataFromGusAction_Execute;
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
    }
}
