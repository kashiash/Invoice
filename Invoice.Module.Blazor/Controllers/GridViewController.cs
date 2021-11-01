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
            }
        }
    }
}
