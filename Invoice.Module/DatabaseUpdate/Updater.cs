using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using Invoice.Module.BusinessObjects;
using Bogus;

namespace Invoice.Module.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            PrzygotujDaneTestowe();
            ObjectSpace.CommitChanges(); //Uncomment this line to persist created object(s).
        }

        private void PrzygotujDaneTestowe()
        {
            var cusFaker = new Faker<Customer>("pl")
                .CustomInstantiator(f => ObjectSpace.CreateObject<Customer>())

                .RuleFor(o => o.Notes, f => f.Company.CatchPhrase())
                .RuleFor(o => o.CustomerName, f => f.Company.CompanyName())

                .RuleFor(o => o.City, f => f.Address.City())
                .RuleFor(o => o.PostalCode, f => f.Address.ZipCode())
                .RuleFor(o => o.Street, f => f.Address.StreetName());
            cusFaker.Generate(100);


            var prodFaker = new Faker<Product>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Product>())
                .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
                .RuleFor(o => o.Notes, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.Symbol, f => f.Commerce.Product())
                .RuleFor(o => o.GTIN, f => f.Commerce.Ean13());

            prodFaker.Generate(100);


            var customers = ObjectSpace.GetObjectsQuery<Customer>(true).ToList();


            var orderFaker = new Faker<Invoice.Module.BusinessObjects.Invoice>("pl")
            .CustomInstantiator(f => ObjectSpace.CreateObject<Invoice.Module.BusinessObjects.Invoice>())
                .RuleFor(o => o.InvoiceNumber, f => f.Random.Int().ToString())
                .RuleFor(o => o.InvoiceDate, f => f.Date.Past(20))
                .RuleFor(o => o.DueDate, f => f.Date.Past(2))
                .RuleFor(o => o.Customer, f => f.PickRandom(customers));
            var orders = orderFaker.Generate(customers.Count * 10);

            var products = ObjectSpace.GetObjectsQuery<Product>(true).ToList();

            var itemsFaker = new Faker<InvoiceItem>()
            .CustomInstantiator(f => ObjectSpace.CreateObject<InvoiceItem>())
                .RuleFor(o => o.Invoice, f => f.PickRandom(orders))
                .RuleFor(o => o.Product, f => f.PickRandom(products))

                .RuleFor(o => o.Quantity, f => f.Random.Decimal(0.01M, 100M))
                .RuleFor(o => o.UnitPrice, f => f.Random.Decimal(0.01M, 100M));
            var items = itemsFaker.Generate(orders.Count * 10);
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
