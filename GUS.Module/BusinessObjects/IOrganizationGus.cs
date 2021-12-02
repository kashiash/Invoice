using System;

namespace GUS.Module.BusinessObjects
{
    public interface IOrganizationGus
    {
        string NumberInRegisterOfRecords { get; set; }
        DateTime DateOfCreation { get; set; }
        DateTime DateOfCommencementBusiness { get; set; }
        DateTime DateOfEntryToRegon { get; set; }
        DateTime DateOfEntryToRegisterOfRecords { get; set; }
        DateTime DateOfChangeOccurrence { get; set; }
        BasicLegalForm BasicLegalForm { get; set; }
        FinancingForm FinancingForm { get; set; }
        FoundingBody FoundingBody { get; set; }
        PropertyForm PropertyForm { get; set; }
        RegistrationAuthority RegistrationAuthority { get; set; }
        RegistryType RegistryType { get; set; }
        SpecificLegalForm SpecificLegalForm { get; set; }
    }
}