using Gus.Regon.BIR11.WebService;
using Gus.Regon.BIR11.Proxy;
using System;
using System.IO;

using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using DevExpress.ExpressApp;

namespace Invoice.Module.Services
{
    public static class GusHelper
    {
        public static Gus.Regon.BIR11.Proxy.Models.DaneSzukajPodmioty.DaneSzukajPodmioty.root GetByNip(string nip)
        {
            var clientOptions = new BirClientOptions()
            {
                EndpointAddress = "https://wyszukiwarkaregon.stat.gov.pl/wsBIR/UslugaBIRzewnPubl.svc",
                UserKey = "f3ccc9d63a3243bba830"
            };

            IOptions<BirClientOptions> optionParameter = Options.Create(clientOptions);
            var client = new Client(optionParameter);
            var loginResponse = client.Zaloguj();

            var searchParameters = new ParametryWyszukiwania { Nip = nip };

            try
            {
                var response = client.DaneSzukajPodmioty(new DaneSzukajPodmiotyRequest { pParametryWyszukiwania = searchParameters });
                if (string.IsNullOrEmpty(response.DaneSzukajPodmiotyResult))
                {
                    throw new UserFriendlyException("Customer not found.");
                }
                using var reader = new StringReader(response.DaneSzukajPodmiotyResult);
                XmlSerializer xmlSerializerData = new XmlSerializer(typeof(Gus.Regon.BIR11.Proxy.Models.DaneSzukajPodmioty.DaneSzukajPodmioty.root));
                return (Gus.Regon.BIR11.Proxy.Models.DaneSzukajPodmioty.DaneSzukajPodmioty.root)xmlSerializerData.Deserialize(reader);
            }
            catch (Exception ex)
            {
                var value = client.GetValue(new GetValueRequest { Body = new GetValueRequestBody { pNazwaParametru = "KomunikatKod" } });
                throw new UserFriendlyException(ex);
            }
            finally
            {
                client.Wyloguj(new WylogujRequest { pIdentyfikatorSesji = loginResponse.ZalogujResult });
            }
        }
    }
}
