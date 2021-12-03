
namespace GUS.Module.BusinessObjects
{
    public interface IAddressGus
    {
        CommuneGus CommuneGus { get; set; }
        CountryGus CountryGus { get; set; }
        CityGus CityGus { get; set; }
        CityGus PostCityGus { get; set; }
        CountyGus CountyGus { get; set; }
        StreetGus StreetGus { get; set; }
        ProvinceGus ProvinceGus { get; set; }
    }
}