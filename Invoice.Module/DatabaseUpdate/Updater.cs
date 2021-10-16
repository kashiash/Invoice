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

            PrepareTestData();
            ObjectSpace.CommitChanges();
        }


        private void PrepareTestData()
        {
            var rates = ObjectSpace.GetObjectsQuery<VatRate>().ToList();
            if(rates.Count == 0)
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
            cusFaker.Generate(10);


            var prodFaker = new Faker<Product>("pl")

            .CustomInstantiator(f => ObjectSpace.CreateObject<Product>())
                .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
                .RuleFor(o => o.Notes, f => f.Commerce.ProductDescription())
                .RuleFor(o => o.Symbol, f => f.Commerce.Product())
                .RuleFor(o => o.UnitPrice, f => f.Random.Decimal(0.01M, 100M))
                .RuleFor(o => o.VatRate, f => f.PickRandom(rates))
                .RuleFor(o => o.GTIN, f => f.Commerce.Ean13());

            prodFaker.Generate(10);


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
            if(vat == null)
            {
                vat = ObjectSpace.CreateObject<VatRate>();
                vat.Symbol = symbol;
                vat.Value = val;
            }
            return vat;
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            base.UpdateDatabaseBeforeUpdateSchema();
        }
    }
}
