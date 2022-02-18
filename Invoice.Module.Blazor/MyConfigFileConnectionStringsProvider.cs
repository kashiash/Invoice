using Common.Module.Utils;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Blazor
{
    public class MyConfigFileConnectionStringsProvider : IDataSourceWizardConnectionStringsProvider
    {
        public Dictionary<string, string> GetConnectionDescriptions()
        {
            Dictionary<string, string> connections = new Dictionary<string, string>();
            connections.Add("dashboardConnection", "Dashboards Connection");
            return connections;
        }
        public DataConnectionParametersBase GetDataConnectionParameters(string name)
        {
            return new CustomStringConnectionParameters(AppSettings.ConnectionString);
        }
    }
}
