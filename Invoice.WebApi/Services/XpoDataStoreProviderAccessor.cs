using System;
using DevExpress.ExpressApp.Xpo;

namespace Invoice.WebApi.Services {
    public class XpoDataStoreProviderAccessor {
        public IXpoDataStoreProvider DataStoreProvider { get; set; }
    }
}
