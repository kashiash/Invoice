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
    public class PkdCode : XPLiteObject
    {
        public PkdCode(Session session) : base(session)
        { }


        string name;
        string pkdCode;
        [Key(false)]
        [Size(10)]
        public string Code
        {
            get => pkdCode;
            set => SetPropertyValue(nameof(Code), ref pkdCode, value);
        }

        
        [Size(250)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
    }
}
