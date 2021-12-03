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
    public class PropertyForm : GUS.Module.BusinessObjects.GusBaseObject
    {
        public PropertyForm(Session session) : base(session)
        { }

        
    }
}
