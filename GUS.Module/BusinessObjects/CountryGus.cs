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
    public class CountryGus : GusBaseObject
    {
        public CountryGus(Session session) : base(session)
        { }

        [Association("CountryGus-Provinces")]

        public XPCollection<ProvinceGus> Provinces
        {
            get
            {
                return GetCollection<ProvinceGus>(nameof(Provinces));
            }
        }
    }
}
