using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Blazor.Components;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DashboardBlazor;
using Microsoft.AspNetCore.Components;


namespace Invoice.Module.Blazor.Controllers
{
    public class IDashboardData_DetailViewController : ObjectViewController<DetailView, IDashboardData>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CustomizeViewItemControl<BlazorDashboardViewerViewItem>(this, CustomizeDashboardViewerViewItem);
        }
        void CustomizeDashboardViewerViewItem(BlazorDashboardViewerViewItem blazorDashboardViewerViewItem)
        {
            if (blazorDashboardViewerViewItem.Control is DxDashboardViewerAdapter dxDashboardViewerAdapter)
            {
                var existingContent = dxDashboardViewerAdapter.ComponentModel.ChildContent;
                dxDashboardViewerAdapter.ComponentModel.ChildContent = builder => {
                    builder.AddContent(1, existingContent);
                    builder.OpenComponent<DxExtensions>(2);
                    RenderFragment dxDataSourceWizardComponent = builder => {
                        builder.OpenComponent<DxDataSourceWizard>(1);
                        builder.AddAttribute(2, nameof(DxDataSourceWizard.EnableCustomSql), true);
                        builder.CloseComponent();
                    };
                    builder.AddAttribute(3, nameof(DxExtensions.ChildContent), dxDataSourceWizardComponent);
                    builder.CloseComponent();
                };
            }
        }
    }
}
