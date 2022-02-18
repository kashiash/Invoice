using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Win;
using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Win.Controllers
{
    public class DashboardDesignerController : ObjectViewController<ObjectView, IDashboardData>
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            WinShowDashboardDesignerController showDashboardDesignerController = Frame.GetController<WinShowDashboardDesignerController>();
            if (showDashboardDesignerController != null)
            {
                showDashboardDesignerController.DashboardDesignerManager.DashboardDesignerCreated += DashboardDesignerManager_DashboardDesignerCreated;
            }
        }
        private void DashboardDesignerManager_DashboardDesignerCreated(object sender, DashboardDesignerShownEventArgs e)
        {
            e.DashboardDesigner.DataSourceWizard.SqlWizardSettings.DatabaseCredentialsSavingBehavior = DevExpress.DataAccess.Wizard.SensitiveInfoSavingBehavior.Always;
        }
        protected override void OnDeactivated()
        {
            WinShowDashboardDesignerController showDashboardDesignerController = Frame.GetController<WinShowDashboardDesignerController>();
            if (showDashboardDesignerController != null)
                showDashboardDesignerController.DashboardDesignerManager.DashboardDesignerCreated -= DashboardDesignerManager_DashboardDesignerCreated;
            base.OnDeactivated();
        }
    }
}
