using DevExpress.ExpressApp;
using GUS.Module.BusinessObjects;
using GusHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUS.Module.Utils
{
    public class GusAddressMapper
    {
        private readonly IObjectSpace objectSpace;
        private readonly DataOfPersonBusiness data;

        public GusAddressMapper(IObjectSpace objectSpace, DataOfPersonBusiness data)
        {
            this.objectSpace = objectSpace;
            this.data = data;
        }

        public void MapGusAddress(IAddressGus addressGus)
        {
            addressGus.CommuneGus = CommuneGus;
            addressGus.CountryGus = CountryGus;
            addressGus.CityGus = CityGus;
            addressGus.PostCityGus = MapPostCityGus();
            addressGus.CountyGus = CountyGus;
            addressGus.StreetGus = MapStreetGus();
            addressGus.ProvinceGus = ProvinceGus;
        }


        private CommuneGus communeGus;
        private CommuneGus CommuneGus
        {
            get
            {
                if (communeGus == null)
                {
                    communeGus = MapCommuneGus();
                }
                return communeGus;
            }
        }
        private CommuneGus MapCommuneGus()
        {
            if (data == null || data.Address == null || data.Address.Commune == null) return null;

            var communeGus = objectSpace.GetObjectsQuery<CommuneGus>(true).Where(x => x.Symbol == data.Address.Commune.Symbol).FirstOrDefault();

            if (communeGus == null)
            {
                communeGus = objectSpace.CreateObject<CommuneGus>();
                communeGus.Name = data.Address.Commune.Name;
                communeGus.Symbol = data.Address.Commune.Symbol;
                communeGus.County = CountyGus;
            }

            return communeGus;
        }


        private CountryGus countryGus;
        private CountryGus CountryGus
        {
            get
            {
                if (countryGus == null)
                {
                    countryGus = MapCountryGus();
                }
                return countryGus;
            }
        }
        private CountryGus MapCountryGus()
        {
            if (data == null || data.Address == null || data.Address.Country == null) return null;

            var countryGus = objectSpace.GetObjectsQuery<CountryGus>(true).Where(x => x.Symbol == data.Address.Country.Symbol).FirstOrDefault();

            if (countryGus == null)
            {
                countryGus = objectSpace.CreateObject<CountryGus>();
                countryGus.Name = data.Address.Country.Name;
                countryGus.Symbol = data.Address.Country.Symbol;
            }

            return countryGus;
        }


        private CityGus cityGus;
        private CityGus CityGus
        {
            get
            {
                if (cityGus == null)
                {
                    cityGus = MapCityGus();
                }
                return cityGus;
            }
        }
        private CityGus MapCityGus()
        {
            if (data == null || data.Address == null || data.Address.City == null) return null;

            var cityGus = objectSpace.GetObjectsQuery<CityGus>(true).Where(x => x.Symbol == data.Address.City.Symbol).FirstOrDefault();

            if (cityGus == null)
            {
                cityGus = objectSpace.CreateObject<CityGus>();
                cityGus.Name = data.Address.City.Name;
                cityGus.Symbol = data.Address.City.Symbol;
                cityGus.Commune = CommuneGus;
            }
            cityGus.Postcode = data.Address.City.Postcode;

            return cityGus;
        }


        private CityGus MapPostCityGus()
        {
            if (data == null || data.Address == null || data.Address.PostOffice == null) return null;

            var cityGus = objectSpace.GetObjectsQuery<CityGus>(true).Where(x => x.Symbol == data.Address.PostOffice.Symbol).FirstOrDefault();

            if (cityGus == null)
            {
                cityGus = objectSpace.CreateObject<CityGus>();
                cityGus.Name = data.Address.PostOffice.Name;
                cityGus.Symbol = data.Address.PostOffice.Symbol;
                cityGus.Commune = CommuneGus;
            }
            cityGus.Postcode = data.Address.City.Postcode;

            return cityGus;
        }


        private CountyGus countyGus;
        private CountyGus CountyGus
        {
            get
            {
                if (countyGus == null)
                {
                    countyGus = MapCountyGus();
                }
                return countyGus;
            }
        }
        private CountyGus MapCountyGus()
        {
            if (data == null || data.Address == null || data.Address.County == null) return null;

            var countyGus = objectSpace.GetObjectsQuery<CountyGus>(true).Where(x => x.Symbol == data.Address.County.Symbol).FirstOrDefault();

            if (countyGus == null)
            {
                countyGus = objectSpace.CreateObject<CountyGus>();
                countyGus.Name = data.Address.County.Name;
                countyGus.Symbol = data.Address.County.Symbol;
                countyGus.Province = ProvinceGus;
            }

            return countyGus;
        }


        private StreetGus MapStreetGus()
        {
            if (data == null || data.Address == null || data.Address.Street == null) return null;

            var streetGus = objectSpace.GetObjectsQuery<StreetGus>(true).Where(x => x.Symbol == data.Address.Street.Symbol).FirstOrDefault();

            if (streetGus == null)
            {
                streetGus = objectSpace.CreateObject<StreetGus>();
                streetGus.Name = data.Address.Street.Name;
                streetGus.Symbol = data.Address.Street.Symbol;
                streetGus.City = CityGus;
                streetGus.ApartmentNumber = data.Address.Street.ApartmentNumber;
                streetGus.PropertyNumber = data.Address.Street.PropertyNumber;
            }

            return streetGus;
        }


        private ProvinceGus provinceGus;
        private ProvinceGus ProvinceGus
        {
            get
            {
                if (provinceGus == null)
                {
                    provinceGus = MapProvinceGus();
                }
                return provinceGus;
            }
        }
        private ProvinceGus MapProvinceGus()
        {
            if (data == null || data.Address == null || data.Address.Province == null) return null;

            var provinceGus = objectSpace.GetObjectsQuery<ProvinceGus>(true).Where(x => x.Symbol == data.Address.Province.Symbol).FirstOrDefault();

            if (provinceGus == null)
            {
                provinceGus = objectSpace.CreateObject<ProvinceGus>();
                provinceGus.Name = data.Address.Province.Name;
                provinceGus.Symbol = data.Address.Province.Symbol;
                provinceGus.Country = CountryGus;
            }

            return provinceGus;
        }
    }
}
