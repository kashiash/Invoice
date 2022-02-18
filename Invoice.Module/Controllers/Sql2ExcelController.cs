using Common.Module.Utils;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
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
    public class Sql2ExcelController : ViewController<ListView>
    {


        Sql2Excel selectedExport;


        private readonly SingleChoiceAction exportSqlResult2ExcelAction;


        public Sql2ExcelController()
        {
            exportSqlResult2ExcelAction = new SingleChoiceAction(this, $"{GetType().FullName}.{nameof(exportSqlResult2ExcelAction)}", PredefinedCategory.Filters);
            exportSqlResult2ExcelAction.Caption = "Eksportuj";
            exportSqlResult2ExcelAction.ToolTip = "Eksportuj do excela";
            exportSqlResult2ExcelAction.ActionMeaning = ActionMeaning.Accept;
            exportSqlResult2ExcelAction.EmptyItemsBehavior = EmptyItemsBehavior.Disable;
            exportSqlResult2ExcelAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            exportSqlResult2ExcelAction.ItemType = SingleChoiceActionItemType.ItemIsOperation;
            exportSqlResult2ExcelAction.EmptyItemsBehavior = EmptyItemsBehavior.Deactivate;
            exportSqlResult2ExcelAction.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            exportSqlResult2ExcelAction.Execute += new SingleChoiceActionExecuteEventHandler(
                FilteringCriterionAction_Execute);


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

        private void FilteringCriterionAction_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            selectedExport = e.SelectedChoiceActionItem.Data as Sql2Excel;
            var rec = View.CurrentObject;
            if (selectedExport != null)
            {
                var query = EvaluateField(ObjectSpace, selectedExport.ZapytanieSQL, rec, selectedExport.ObjectType);
                IObjectSpace newObjectSpace = Application.CreateObjectSpace();
                var newObject = newObjectSpace.CreateObject<DemoParameters>();
                newObject.Query = query;
                newObject.OdDnia = DateTime.Now;
                newObject.DoDnia = DateTime.Now;

                e.ShowViewParameters.CreatedView = Application.CreateDetailView(newObjectSpace, newObject);
                e.ShowViewParameters.TargetWindow = TargetWindow.NewModalWindow;
                e.ShowViewParameters.Context = TemplateContext.PopupWindow;
                DialogController dc = Application.CreateController<DialogController>();
                dc.Accepting += new EventHandler<DialogControllerAcceptingEventArgs>(dc_Accepting);
                e.ShowViewParameters.Controllers.Add(dc);

            }
            
        }
        void dc_Accepting(object sender, DialogControllerAcceptingEventArgs e)
        {
      

            View popupView = ((Controller)sender).Frame.View;
            var parameters = popupView.CurrentObject as DemoParameters;

            var filename = $"Test{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
            EksportujDane(ReplaceParams(parameters.Query,parameters.OdDnia,parameters.DoDnia), filename);
        }

        private string ReplaceParams(string query, DateTime odDnia, DateTime doDnia)
        {
            var query1 = query.Replace("?OdDnia", $"'{odDnia.ToString("yyyy-MM-dd")}'");
            var query2 = query1.Replace("?DoDnia", $"'{doDnia.ToString("yyyy-MM-dd")}'");
            return query2;
        }

        private void RefreshActionItems()
        {
            exportSqlResult2ExcelAction.Items.Clear();


            var exports = ObjectSpace.GetObjects<Sql2Excel>().OrderBy(e => e.Nazwa);


            foreach (var exporter in exports)
            {
                if (exporter.ObjectType != null &&
                    exporter.ObjectType.IsAssignableFrom(View.ObjectTypeInfo.Type))
                {
                    exportSqlResult2ExcelAction.Items.Add(new ChoiceActionItem(exporter.Nazwa, exporter));
                }
            }


        }




        private void View_ModelSaved(object sender, EventArgs e) { RefreshActionItems(); }


        public static bool EksportujDane(string zapytanie, string filename)
        {



            Session _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            using (UnitOfWork unitOfWork = new UnitOfWork(_session.DataLayer))
            {

                var rekordy = GetRecordList(unitOfWork, zapytanie);


                int rows = rekordy.ResultSet[0].Rows.Count();
                if (rows == 0)
                    return false;

                Console.WriteLine($"Liczba rekordów w wyniku : {rows}");

                using (var book = new DevExpress.Spreadsheet.Workbook())
                {
                    book.CreateNewDocument();

                    int row = 0;
                    foreach (var rec in rekordy.ResultSet[0].Rows)
                    {

                        var val = rec.Values[0];
                        WriteCell(book, 0, row, val);
                        row++;
                    }
                    int lk = 0;
                    foreach (var rec in rekordy.ResultSet[1].Rows)
                    {
                        lk++;
                        int colls = rec.Values.Length;

                        for (int i = 0; i < colls; i++)
                        {
                            var val = rec.Values[i];
                            WriteCell(book, lk, i, val);

                        }
                    }
           
                    book.SaveDocument(filename, DocumentFormat.OpenXml);

                    var startInfo = new ProcessStartInfo($"{filename}")
                    {
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);


                }

                return true;
            }


        }


        private static void WriteCell(Workbook book, int row, int col, Object val)
        {
            if (val != null && val is not Guid)
            {
                book.Worksheets[0].Cells[row, col].SetValue(val);
            }
        }



        private static SelectedData GetRecordList(Session session, string zapytanie)
        {
            var res = session.ExecuteQueryWithMetadata(zapytanie);
            return res;
        }

        private string EvaluateField(IObjectSpace objectSpace, string expression, Object rec, Type criteriaObjectType)
        {
            if (string.IsNullOrEmpty(expression))
            {
                return string.Empty;
            }
            try
            {
                if (expression.Contains("!"))
                    expression = expression.Replace("!", string.Empty);

                ExpressionEvaluator evaluator = objectSpace.GetExpressionEvaluator(
                    criteriaObjectType,
                    CriteriaOperator.Parse(expression));
                string subject = evaluator.Evaluate(rec).ToString();
                return subject;
            }
            catch (Exception ex)
            {
                throw new Exception($"Bład wyrażenia:{expression}", ex);
            }
        }

    }

    [DomainComponent]
    public class DemoParameters
    {
        public DateTime OdDnia { get; set; }
        public DateTime DoDnia { get; set; }
        [Browsable(false)]
        public string Query { get; set; }
    }
}

