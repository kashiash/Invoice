using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUS.Module.BusinessObjects
{

    [NavigationItem("GUS")]
    [DefaultClassOptions]
    public class RegistryType : GUS.Module.BusinessObjects.GusBaseObject
    {
        public RegistryType(Session session) : base(session)
        { }

        
    }
}
