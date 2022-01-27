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
    public class CityGus : GusBaseObject
    {
        public CityGus(Session session) : base(session) { }

        string postcode;
        CommuneGus commune;

        [Association("CommuneGus-Cities")]
        public CommuneGus Commune
        {
            get => commune;
            set => SetPropertyValue(nameof(Commune), ref commune, value);
        }

        [Association("CityGus-Streets")]
        public XPCollection<StreetGus> Streets
        {
            get
            {
                return GetCollection<StreetGus>(nameof(Streets));
            }
        }

        public string Postcode
        {
            get => postcode;
            set => SetPropertyValue(nameof(Postcode), ref postcode, value);
        }
    }
}