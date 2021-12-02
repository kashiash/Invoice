using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors.Grid;

namespace Invoice.Module.Blazor.Controllers
{
    public class GridViewController : ViewController<ListView>
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is GridListEditor gridListEditor)
            {
                IDxDataGridAdapter dataGridAdapter = gridListEditor.GetDataGridAdapter();
                dataGridAdapter.DataGridModel.CssClass += " table-striped";

                dataGridAdapter.DataGridModel.ColumnResizeMode = DataGridColumnResizeMode.Component;
                dataGridAdapter.DataGridModel.ShowFilterRow = true;
                foreach (var column in gridListEditor.Columns)
                {
                    if (column.Width < 150)
                    {
                        column.Width = 150;
                    }
                }
            }
        }
    }
}
