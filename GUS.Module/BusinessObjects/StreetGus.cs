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
    public class StreetGus : GusBaseObject
    {
        public StreetGus(Session session) : base(session)
        { }


        CityGus city;

        [Association("CityGus-Streets")]
        public CityGus City
        {
            get => city;
            set => SetPropertyValue(nameof(City), ref city, value);
        }
    }
}
