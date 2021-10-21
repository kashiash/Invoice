﻿using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Invoice.Module.BusinessObjects;
using Bogus;

namespace Invoice.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FirstOrDefault<DomainObject1>(u => u.Name == name);
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}
			CreateDefaultRole();
            string autogeneratedAdminUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            ApplicationUser userAdmin = ObjectSpace.FirstOrDefault<ApplicationUser>(u => u.UserName == autogeneratedAdminUserName);
            if(userAdmin == null) {
                userAdmin = ObjectSpace.CreateObject<ApplicationUser>();
                userAdmin.UserName = autogeneratedAdminUserName;
                // Set a password if the standard authentication type is used
                userAdmin.SetPassword("");

                // The UserLoginInfo object requires a user object Id (Oid).
                // Commit the user object to the database before you create a UserLoginInfo object. This will correctly initialize the user key property.
                ObjectSpace.CommitChanges(); //This line persists created object(s).

                //((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, ObjectSpace.GetKeyValueAsString(userAdmin));
                ((ISecurityUserWithLoginInfo)userAdmin).CreateUserLoginInfo(SecurityDefaults.WindowsAuthentication, autogeneratedAdminUserName);
            }
			// If a role with the Administrators name doesn't exist in the database, create this role
            PermissionPolicyRole adminRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(r => r.Name == "Administrators");
            if(adminRole == null) {
                adminRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                adminRole.Name = "Administrators";
            }
            adminRole.IsAdministrative = true;
			userAdmin.Roles.Add(adminRole);


            //  PrepareTestData();

            ObjectSpace.CommitChanges(); //This line persists created object(s).
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }

        private void PrepareTestData()
        {
            var rates = ObjectSpace.GetObjectsQuery<VatRate>().ToList();
            if (rates.Count == 0)
            {
                rates.Add(NowaStawka("23%", 23M));
                rates.Add(NowaStawka("0%", 0M));
                rates.Add(NowaStawka("7%", 7M));
                rates.Add(NowaStawka("ZW", 0M));
            }

            var cusFaker = new Faker<Customer>("pl")
                .CustomInstantiator(f => ObjectSpace.CreateObject<Customer>())

                .RuleFor(o => o.Notes, f => f.Company.CatchPhrase())
                .RuleFor(o => o.CustomerName, f => f.Company.CompanyName())
                .RuleFor(o => o.Segment, f => f.PickRandom<Segment>())
                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.PostalCode, f => f.Address.ZipCode())
                .RuleFor(o => o.Street, f => f.Address.StreetName())
                .RuleFor(o => o.Phone, f => f.Person.Phone)
                .RuleFor(o => o.Email, (f, c) => f.Internet.Email());
            cusFaker.Generate(100);


            var prodFaker = new Faker<Product>("pl")

            .CustomInstantiator(f => ObjectSpace.CreateObject<Product>())
                .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
                .RuleFor(o => o.Notes, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.Symbol, f => f.Commerce.Product())
                .RuleFor(o => o.UnitPrice, f => f.Random.Decimal(0.01M, 100M))
                .RuleFor(o => o.VatRate, f => f.PickRandom(rates))
                .RuleFor(o => o.GTIN, f => f.Commerce.Ean13());

            prodFaker.Generate(100);


            var customers = ObjectSpace.GetObjectsQuery<Customer>(true).ToList();


            var orderFaker = new Faker<Invoice.Module.BusinessObjects.Invoice>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Invoice.Module.BusinessObjects.Invoice>())
                .RuleFor(o => o.InvoiceNumber, f => f.Random.Int().ToString())
                .RuleFor(o => o.InvoiceDate, f => f.Date.Past(2))
                .RuleFor(o => o.DueDate, f => f.Date.Past(2))
                .RuleFor(o => o.Customer, f => f.PickRandom(customers));
            var orders = orderFaker.Generate(customers.Count * 10);

            var products = ObjectSpace.GetObjectsQuery<Product>(true).ToList();

            var itemsFaker = new Faker<InvoiceItem>()
            .CustomInstantiator(f => ObjectSpace.CreateObject<InvoiceItem>())
                .RuleFor(o => o.Invoice, f => f.PickRandom(orders))
                .RuleFor(o => o.Product, f => f.PickRandom(products))
                .RuleFor(o => o.Quantity, f => f.Random.Decimal(0.01M, 100M));

            var items = itemsFaker.Generate(orders.Count * 10);
        }

        private VatRate NowaStawka(string symbol, decimal val)
        {
            var vat = ObjectSpace.FindObject<VatRate>(CriteriaOperator.Parse("Symbol = ?", symbol));
            if (vat == null)
            {
                vat = ObjectSpace.CreateObject<VatRate>();
                vat.Symbol = symbol;
                vat.Value = val;
            }
            return vat;
        }
        private PermissionPolicyRole CreateDefaultRole() {
            PermissionPolicyRole defaultRole = ObjectSpace.FirstOrDefault<PermissionPolicyRole>(role => role.Name == "Default");
            if(defaultRole == null) {
                defaultRole = ObjectSpace.CreateObject<PermissionPolicyRole>();
                defaultRole.Name = "Default";

				defaultRole.AddObjectPermissionFromLambda<PermissionPolicyUser>(SecurityOperations.Read, cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails", SecurityPermissionState.Allow);
				defaultRole.AddMemberPermissionFromLambda<PermissionPolicyUser>(SecurityOperations.Write, "ChangePasswordOnFirstLogon", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
				defaultRole.AddMemberPermissionFromLambda<PermissionPolicyUser>(SecurityOperations.Write, "StoredPassword", cm => cm.Oid == (Guid)CurrentUserIdOperator.CurrentUserId(), SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<PermissionPolicyRole>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess, SecurityPermissionState.Allow);
				defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create, SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create, SecurityPermissionState.Allow);
            }
            return defaultRole;
        }
    }
}
