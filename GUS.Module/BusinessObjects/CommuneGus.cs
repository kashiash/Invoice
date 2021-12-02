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
    public class CommuneGus : GusBaseObject
    {
        public CommuneGus(Session session) : base(session)
        { }



        CountyGus county;

        [Association("CountyGus-Communes")]
        public CountyGus County
        {
            get => county;
            set => SetPropertyValue(nameof(County), ref county, value);
        }



        [Association("CommuneGus-Cities")]
        public XPCollection<CityGus> Cities
        {
            get
            {
                return GetCollection<CityGus>(nameof(Cities));
            }
        }
    }
}
