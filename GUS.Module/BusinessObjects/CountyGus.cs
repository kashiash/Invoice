using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace GUS.Module.BusinessObjects
{
    [NavigationItem("GUS")]
    [DefaultClassOptions]
    public class CountyGus : GusBaseObject
    {
        public CountyGus(Session session) : base(session)
        { }



        ProvinceGus province;

        [Association("ProvinceGus-Counties")]
        public ProvinceGus Province
        {
            get => province;
            set => SetPropertyValue(nameof(Province), ref province, value);
        }

        [Association("CountyGus-Communes")]
        public XPCollection<CommuneGus> Comunes
        {
            get
            {
                return GetCollection<CommuneGus>(nameof(Comunes));
            }
        }

    }
}
