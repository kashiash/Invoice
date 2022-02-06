using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using Invoice.Module.BusinessObjects;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers.KontroleryDemonstracyjne
{
    //public class InvoiceControllerShowReport : ObjectViewController<ListView, Invoice.Module.BusinessObjects.Invoice>
    public class InvoiceControllerShowReport : ViewController
    {


        private readonly SimpleAction drukujFaktureAction;

        public InvoiceControllerShowReport()
        {
            TargetObjectType = typeof(Invoice.Module.BusinessObjects.Invoice);

            drukujFaktureAction = new SimpleAction(this, nameof(drukujFaktureAction), PredefinedCategory.Reports)
            {
                Caption = "Print selected record",
                ImageName = "PrintQuick",
                PaintStyle = ActionItemPaintStyle.CaptionAndImage,
                SelectionDependencyType = SelectionDependencyType.RequireSingleObject
            };
            drukujFaktureAction.Execute += UmowaNajmuAction_Execute;
        }

        private void UmowaNajmuAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            IReportDataV2 reportData = objectSpace.FirstOrDefault<ReportDataV2>(x => x.DisplayName == "Invoice"); 

            if (reportData != null && View != null && View.SelectedObjects.Count > 0)
            {
                var report = ReportDataProvider.ReportsStorage.LoadReport(reportData);
                ReportsModuleV2 reportsModule = ReportsModuleV2.FindReportsModule(Application.Modules);

                var najem = View.SelectedObjects[0] as Invoice.Module.BusinessObjects.Invoice;
                CriteriaOperator criteria = CriteriaOperator.FromLambda<Invoice.Module.BusinessObjects.Invoice>(z => z.Oid == najem.Oid);
                reportsModule.ReportsDataSourceHelper.SetupBeforePrint(report);

                string handle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
                ReportServiceController controller = Frame.GetController<ReportServiceController>();
                if (controller != null)
                {
                    controller.ShowPreview(handle, criteria);
                }
            }
        }

    } 
}
