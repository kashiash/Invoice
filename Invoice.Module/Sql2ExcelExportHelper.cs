using Common.Module.Utils;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module
{
    /// <summary>
    /// Require DevExpress.ExpressApp.Office package
    /// </summary>
    public static class Sql2ExcelExportHelper
    {
        public static bool ExportData(string zapytanie, string destinationFilename)
        {

            Session _session = new Session() { ConnectionString = AppSettings.ConnectionString };
            using (UnitOfWork unitOfWork = new UnitOfWork(_session.DataLayer))
            {
                var rekordy = GetRecordList(unitOfWork, zapytanie);

                int rows = rekordy.ResultSet[0].Rows.Count();
                if (rows == 0) return false;

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

                    var filename = $"{destinationFilename}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                    book.SaveDocument(filename, DocumentFormat.Xlsx);

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

        public static string EvaluateField(IObjectSpace objectSpace, string expression, Object rec, Type criteriaObjectType)
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
}
