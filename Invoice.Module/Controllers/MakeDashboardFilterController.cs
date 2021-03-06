using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers
{
    public class CurrencyDashboardFilterController : ViewController<DashboardView>
    {
        private const string DashboardViewId = "MakeModelsDashboardView";
        private DashboardViewItem MakeItem;

        private DashboardViewItem ModelItem;

        private DashboardViewItem TrimItem;



        private void FilterModelListView(ListView masterListView, ListView detailListView)
        {
            detailListView.CollectionSource.Criteria.Clear();
            List<object> searchedObjects = new List<object>();
            foreach (object obj in masterListView.SelectedObjects)
            {
                searchedObjects.Add(detailListView.ObjectSpace.GetKeyValue(obj));
            }
            if (searchedObjects.Count > 0)
            {
                detailListView.CollectionSource.Criteria["SelectedMake"] = new InOperator("id_car_make.id_car_make", searchedObjects);
            }
        }

        private void FilterTrimListView(ListView masterListView, ListView detailListView)
        {
            detailListView.CollectionSource.Criteria.Clear();
            List<object> searchedObjects = new List<object>();
            foreach (object obj in masterListView.SelectedObjects)
            {
                searchedObjects.Add(detailListView.ObjectSpace.GetKeyValue(obj));
            }
            if (searchedObjects.Count > 0)
            {
                detailListView.CollectionSource.Criteria["SelectedModel"] = new InOperator("id_car_model.id_car_model", searchedObjects);
            }
        }


        private void MakeItem_ControlCreated(object sender, EventArgs e)
        {
            DashboardViewItem dashboardItem = (DashboardViewItem)sender;
            ListView innerListView = dashboardItem.InnerView as ListView;
            if (innerListView != null)
            {
                innerListView.SelectionChanged -= makeListView_SelectionChanged;
                innerListView.SelectionChanged += makeListView_SelectionChanged;
            }
        }
        private void makeListView_SelectionChanged(object sender, EventArgs e)
        {

            FilterModelListView((ListView)MakeItem.InnerView, (ListView)ModelItem.InnerView);

        }
        private void DisableNavigationActions(Frame frame)
        {
            RecordsNavigationController recordsNavigationController = frame.GetController<RecordsNavigationController>();
            if (recordsNavigationController != null)
            {
                recordsNavigationController.Active.SetItemValue("DashboardFiltering", false);
            }
        }

        private void ModelItem_ControlCreated(object sender, EventArgs e)
        {
            DashboardViewItem dashboardItem = (DashboardViewItem)sender;
            ListView innerListView = dashboardItem.InnerView as ListView;
            if (innerListView != null)
            {
                innerListView.SelectionChanged -= modelListView_SelectionChanged;
                innerListView.SelectionChanged += modelListView_SelectionChanged;
            }
        }

        private void modelListView_SelectionChanged(object sender, EventArgs e)
        {

            FilterTrimListView((ListView)ModelItem.InnerView, (ListView)TrimItem.InnerView);

        }




        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.Id == DashboardViewId)
            {
                MakeItem = (DashboardViewItem)View.FindItem(FilterMakeID);
                ModelItem = (DashboardViewItem)View.FindItem(FilterModelId);
                TrimItem = (DashboardViewItem)View.FindItem(FilterTrimId);


                if (MakeItem != null)
                {
                    MakeItem.ControlCreated += MakeItem_ControlCreated;
                }
                if (ModelItem != null)
                {
                    if (ModelItem.Frame != null)
                        DisableNavigationActions(ModelItem.Frame);
                
                        ModelItem.ControlCreated += ModelItem_ControlCreated;
                }
                if (TrimItem != null)
                {
                    if (TrimItem.Frame != null)
                        DisableNavigationActions(TrimItem.Frame);

                }

            }
        }



        protected override void OnDeactivated()
        {
            if (MakeItem != null)
            {
                MakeItem.ControlCreated -= MakeItem_ControlCreated;
                MakeItem = null;
            }
            if (ModelItem != null)
            {
                ModelItem.ControlCreated -= ModelItem_ControlCreated;
                ModelItem = null;
            }
            if (TrimItem != null)
            {

                TrimItem = null;
            }

            base.OnDeactivated();
        }



        public CurrencyDashboardFilterController()
        {
            FilterMakeID = "Make";
            FilterModelId = "Model";
            FilterTrimId = "Trim";

        }
        public string FilterMakeID { get; set; }
        public string FilterModelId { get; set; }
        public string FilterTrimId { get; set; }

    }
}
