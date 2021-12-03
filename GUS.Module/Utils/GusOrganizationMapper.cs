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
    public class GusOrganizationMapper
    {
        private readonly IObjectSpace objectSpace;
        private readonly DataOfPersonBusiness data;

        public GusOrganizationMapper(IObjectSpace objectSpace, DataOfPersonBusiness data)
        {
            this.objectSpace = objectSpace;
            this.data = data;
        }

        public void MapGusOrganization(IOrganizationGus organizationGus)
        {
            organizationGus.NumberInRegisterOfRecords = data.NumberInRegisterOfRecords;
            organizationGus.DateOfCreation = data.DateOfCreation;
            organizationGus.DateOfCommencementBusiness = data.DateOfCommencementBusiness;
            organizationGus.DateOfEntryToRegon = data.DateOfEntryToRegon;
            organizationGus.DateOfEntryToRegisterOfRecords = data.DateOfEntryToRegisterOfRecords;
            organizationGus.DateOfChangeOccurrence = data.DateOfChangeOccurrence;
            organizationGus.BasicLegalForm = MapBasicLegalForm();
            organizationGus.FinancingForm = MapFinancingForm();
            organizationGus.FoundingBody = MapFoundingBody();
            organizationGus.PropertyForm = MapPropertyForm();
            organizationGus.RegistrationAuthority = MapRegistrationAuthority();
            organizationGus.RegistryType = MapRegistryType();
            organizationGus.SpecificLegalForm = MapSpecificLegalForm();
        }

        private BasicLegalForm MapBasicLegalForm()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.BasicLegalForm == null) return null;

            var basicLegalForm = objectSpace.GetObjectsQuery<BasicLegalForm>(true).Where(x => x.Symbol == data.OrganizationData.BasicLegalForm.Symbol).FirstOrDefault();

            if (basicLegalForm == null)
            {
                basicLegalForm = objectSpace.CreateObject<BasicLegalForm>();
                basicLegalForm.Name = data.OrganizationData.BasicLegalForm.Name;
                basicLegalForm.Symbol = data.OrganizationData.BasicLegalForm.Symbol;
            }

            return basicLegalForm;
        }

        private FinancingForm MapFinancingForm()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.FinancingForm == null) return null;

            var financingForm = objectSpace.GetObjectsQuery<FinancingForm>(true).Where(x => x.Symbol == data.OrganizationData.FinancingForm.Symbol).FirstOrDefault();

            if (financingForm == null)
            {
                financingForm = objectSpace.CreateObject<FinancingForm>();
                financingForm.Name = data.OrganizationData.FinancingForm.Name;
                financingForm.Symbol = data.OrganizationData.FinancingForm.Symbol;
            }

            return financingForm;
        }

        private FoundingBody MapFoundingBody()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.FoundingBody == null) return null;

            var foundingBody = objectSpace.GetObjectsQuery<FoundingBody>(true).Where(x => x.Symbol == data.OrganizationData.FoundingBody.Symbol).FirstOrDefault();

            if (foundingBody == null)
            {
                foundingBody = objectSpace.CreateObject<FoundingBody>();
                foundingBody.Name = data.OrganizationData.FoundingBody.Name;
                foundingBody.Symbol = data.OrganizationData.FoundingBody.Symbol;
            }

            return foundingBody;
        }

        private PropertyForm MapPropertyForm()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.PropertyForm == null) return null;

            var propertyForm = objectSpace.GetObjectsQuery<PropertyForm>(true).Where(x => x.Symbol == data.OrganizationData.PropertyForm.Symbol).FirstOrDefault();

            if (propertyForm == null)
            {
                propertyForm = objectSpace.CreateObject<PropertyForm>();
                propertyForm.Name = data.OrganizationData.PropertyForm.Name;
                propertyForm.Symbol = data.OrganizationData.PropertyForm.Symbol;
            }

            return propertyForm;
        }

        private RegistrationAuthority MapRegistrationAuthority()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.RegistrationAuthority == null) return null;

            var registrationAuthority = objectSpace.GetObjectsQuery<RegistrationAuthority>(true).Where(x => x.Symbol == data.OrganizationData.RegistrationAuthority.Symbol).FirstOrDefault();

            if (registrationAuthority == null)
            {
                registrationAuthority = objectSpace.CreateObject<RegistrationAuthority>();
                registrationAuthority.Name = data.OrganizationData.RegistrationAuthority.Name;
                registrationAuthority.Symbol = data.OrganizationData.RegistrationAuthority.Symbol;
            }

            return registrationAuthority;
        }

        private RegistryType MapRegistryType()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.RegistryType == null) return null;

            var registryType = objectSpace.GetObjectsQuery<RegistryType>(true).Where(x => x.Symbol == data.OrganizationData.RegistryType.Symbol).FirstOrDefault();

            if (registryType == null)
            {
                registryType = objectSpace.CreateObject<RegistryType>();
                registryType.Name = data.OrganizationData.RegistryType.Name;
                registryType.Symbol = data.OrganizationData.RegistryType.Symbol;
            }

            return registryType;
        }

        private SpecificLegalForm MapSpecificLegalForm()
        {
            if (data == null || data.OrganizationData == null || data.OrganizationData.SpecificLegalForm == null) return null;

            var specificLegalForm = objectSpace.GetObjectsQuery<SpecificLegalForm>(true).Where(x => x.Symbol == data.OrganizationData.SpecificLegalForm.Symbol).FirstOrDefault();

            if (specificLegalForm == null)
            {
                specificLegalForm = objectSpace.CreateObject<SpecificLegalForm>();
                specificLegalForm.Name = data.OrganizationData.SpecificLegalForm.Name;
                specificLegalForm.Symbol = data.OrganizationData.SpecificLegalForm.Symbol;
            }

            return specificLegalForm;
        }
    }
}
