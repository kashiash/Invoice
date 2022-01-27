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
        public StreetGus(Session session) : base(session) { }

        string propertyNumber;
        string apartmentNumber;
        CityGus city;

        [Association("CityGus-Streets")]
        public CityGus City
        {
            get => city;
            set => SetPropertyValue(nameof(City), ref city, value);
        }

        public string ApartmentNumber
        {
            get => apartmentNumber;
            set => SetPropertyValue(nameof(ApartmentNumber), ref apartmentNumber, value);
        }

        public string PropertyNumber
        {
            get => propertyNumber;
            set => SetPropertyValue(nameof(PropertyNumber), ref propertyNumber, value);
        }
    }
}
