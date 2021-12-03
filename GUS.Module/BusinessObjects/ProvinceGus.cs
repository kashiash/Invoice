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
    public class ProvinceGus : GusBaseObject
    {
        public ProvinceGus(Session session) : base(session)
        { }


        [Association("ProvinceGus-Counties")]
        public XPCollection<CountyGus> Counties
        {
            get
            {
                return GetCollection<CountyGus>(nameof(Counties));
            }
        }

        CountryGus country;

        [Association("CountryGus-Provinces")]
        public CountryGus Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }
    }
}
