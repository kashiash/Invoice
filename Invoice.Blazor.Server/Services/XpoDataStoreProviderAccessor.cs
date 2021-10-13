using System;
using DevExpress.ExpressApp.Xpo;

namespace Invoice.Blazor.Server.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
