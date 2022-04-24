using Common.Module.Utils;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Invoice.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers
{
    /// <summary>
    /// Require DevExpress.ExpressApp.Office package
    /// </summary>
    public class Sql2ExcelController : ViewController<ListView>
    {


        Sql2Excel selectedExport;


        private readonly SingleChoiceAction exportSqlResult2ExcelAction;


        public Sql2ExcelController()
        {
            exportSqlResult2ExcelAction = new SingleChoiceAction(this, $"{GetType().FullName}.{nameof(exportSqlResult2ExcelAction)}", PredefinedCategory.Filters)
            {
                Caption = "Export to Excel",
                ToolTip = "Export to Excel",
                ImageName = "ExportToXLSX",
                ImageMode = ImageMode.UseActionImage,
                ActionMeaning = ActionMeaning.Unknown,
                EmptyItemsBehavior = EmptyItemsBehavior.Deactivate,
                PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage,
                ItemType = SingleChoiceActionItemType.ItemIsOperation,
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects
            }
        ;
            exportSqlResult2ExcelAction.Execute += new SingleChoiceActionExecuteEventHandler(
                ExportAction_Execute);
        }

        public void RefreshFilter(string activeFilterString)
        {
            if (View.Model.Filter == activeFilterString)
            {
                return;
            }

            RefreshActionItems();

        }

        protected override void OnActivated()
        {

            RefreshActionItems();

        }

        private void ExportAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            selectedExport = e.SelectedChoiceActionItem.Data as Sql2Excel;
            var rec = View.CurrentObject;
            if (selectedExport != null)
            {
                var query = Sql2ExcelExportHelper.EvaluateField(ObjectSpace, selectedExport.SqlQueryExpression, rec, selectedExport.ObjectType);
                var filename = Sql2ExcelExportHelper.EvaluateField(ObjectSpace, selectedExport.ExportedFileName, rec, selectedExport.ObjectType);

                if (selectedExport.ParametersObjectType == null)
                {

                    Sql2ExcelExportHelper.ExportData(query, filename);
                }
                else
                {

                    var par = selectedExport.ParametersObjectType;



                    IObjectSpace newObjectSpace = Application.CreateObjectSpace();
                    var paramsView = newObjectSpace.CreateObject(par);

                    ((ExportParametersObjectBase)paramsView).Query = query;
                    ((ExportParametersObjectBase)paramsView).FileName = filename;
                    ((ExportParametersObjectBase)paramsView).Description = selectedExport.Description;


                    e.ShowViewParameters.CreatedView = Application.CreateDetailView(newObjectSpace, paramsView);
                    e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                    e.ShowViewParameters.Context = TemplateContext.PopupWindow;
                    DialogController dc = Application.CreateController<DialogController>();
                    dc.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dc_Accepting);
                    e.ShowViewParameters.Controllers.Add(dc);

                }
            }

        }
        void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {


            View popupView = ((Controller)sender).Frame.View;
            var parameters = popupView.CurrentObject as ExportParametersObjectBase;


            Sql2ExcelExportHelper.ExportData(parameters.GetQuery(), parameters.FileName);
        }



        private void RefreshActionItems()
        {
            exportSqlResult2ExcelAction.Items.Clear();
            var exports = ObjectSpace.GetObjects<Sql2Excel>().Where(e => e.InPlace && !e.Archived).OrderBy(e => e.Name);

            foreach (var exporter in exports)
            {
                if (exporter.ObjectType != null &&
                    exporter.ObjectType.IsAssignableFrom(View.ObjectTypeInfo.Type))
                {
                    exportSqlResult2ExcelAction.Items.Add(new ChoiceActionItem(exporter.Name, exporter));
                }
            }
        }
        private void View_ModelSaved(object sender, EventArgs e) { RefreshActionItems(); }
    }


}

