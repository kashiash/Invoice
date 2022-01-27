using DevExpress.ExpressApp;
using GUS.Module.BusinessObjects;
using GUS.Module.Utils;
using GusHelper;
using GusHelper.ViewModels;
using Invoice.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Services
{
    public class GusService
    {
        private readonly IObjectSpace objectSpace;

        public GusService(IObjectSpace objectSpace)
        {
            this.objectSpace = objectSpace;
        }

        public void GetDataFromGus(Customer customer)
        {
            var nip = customer.VatNumber;
            while (nip.Contains("-")) nip = nip.Replace("-", "");

            var gusHelperClient = new GusHelperClient("f3ccc9d63a3243bba830");
            DataOfPersonBusiness data = null;
            try
            {
                data = Task.Run(() => gusHelperClient.GetFullReport(nip)).Result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }

            if (data == null) return;

            var gusAddressMapper = new GusAddressMapper(objectSpace, data);
            var gusOrganizationMapper = new GusOrganizationMapper(objectSpace, data);

            gusOrganizationMapper.MapGusOrganization(customer);
            gusAddressMapper.MapGusAddress(customer);

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                customer.CustomerName = data.Name;
            }
            if (string.IsNullOrWhiteSpace(customer.Symbol))
            {
                customer.Symbol = data.ShortName;
            }
            if (string.IsNullOrWhiteSpace(customer.City))
            {
                customer.City = customer.CityGus?.Name;
            }
            if (string.IsNullOrWhiteSpace(customer.Street))
            {
                customer.Street = customer.StreetGus?.Name;
            }
            if (string.IsNullOrWhiteSpace(customer.Email))
            {
                customer.Email = data.Email;
            }
            if (string.IsNullOrWhiteSpace(customer.Phone))
            {
                customer.Phone = data.PhoneNumber;
            }
            if (string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                customer.CustomerName = data.Name;
            }
            if (string.IsNullOrWhiteSpace(customer.PostalCode))
            {
                customer.PostalCode = customer.CityGus?.Postcode;
            }
            MapPkdList(customer, data);
        }

        private void MapPkdList(Customer customer, DataOfPersonBusiness data)
        {
            foreach (var pkd in data.PkdList)
            {
                if (string.IsNullOrWhiteSpace(pkd.Code)) continue;

                var pkdCode = objectSpace.GetObjectsQuery<PkdCode>(true).Where(x => x.Code == pkd.Code).FirstOrDefault();
                if (pkdCode == null)
                {
                    pkdCode = objectSpace.CreateObject<PkdCode>();
                    pkdCode.Name = pkd.Name;
                    pkdCode.Code = pkd.Code;
                }

                var customerPkd = objectSpace.GetObjectsQuery<CustomerPkd>(true)
                    .Where(x => x.Customer.Oid == customer.Oid && x.PkdCode.Code == pkd.Code)
                    .FirstOrDefault();

                if (customerPkd == null)
                {
                    customerPkd = objectSpace.CreateObject<CustomerPkd>();
                    customerPkd.Customer = customer;
                    customerPkd.PkdCode = pkdCode;
                }
            }
        }
    }
}
