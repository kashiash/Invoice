using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Templates;
using DevExpress.Persistent.Base;
using Invoice.Module.BusinessObjects;
using Invoice.Module.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers
{
    public class CustomerListViewController : ObjectViewController<ListView, Customer>
    {
        private readonly ParametrizedAction searchCustomerAction;

        public CustomerListViewController()
        {
            searchCustomerAction = new ParametrizedAction(this, $"{GetType().FullName}{nameof(searchCustomerAction)}", PredefinedCategory.Unspecified, typeof(string))
            {
                Caption = "Search customer",
                ImageName = "EnableSearch",
                Shortcut = "F6",
                PaintStyle = ActionItemPaintStyle.Image,
            };
            searchCustomerAction.Execute += SearchCustomerAction_Execute;
        }

        private void SearchCustomerAction_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            if (e.ParameterCurrentValue == null)
            {
                return;
            }
            if (e.ParameterCurrentValue.ToString().Length != 10)
            {
                throw new UserFriendlyException("Vat number must have 10 signs.");
            }

            var objectSpace = Application.CreateObjectSpace();
            var customer = objectSpace.GetObjectsQuery<Customer>().Where(x => x.VatNumber == e.ParameterCurrentValue.ToString()).FirstOrDefault();
            if (customer == null)
            {
                customer = SearchCustomer(e, objectSpace);
            }

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(objectSpace, customer);
        }

        private Customer SearchCustomer(ParametrizedActionExecuteEventArgs e, IObjectSpace objectSpace)
        {
            Customer customer;
            var subject = GusHelper.GetByNip(e.ParameterCurrentValue.ToString())?.dane?.FirstOrDefault();
            if (subject == null || (subject.Regon == null && subject.Nazwa == null && subject.Miejscowosc == null))
            {
                throw new UserFriendlyException("Customer not found.");
            }

            customer = objectSpace.CreateObject<Customer>();
            customer.VatNumber = subject.Nip;
            customer.PostalCode = subject.KodPocztowy;
            customer.City = subject.Miejscowosc;
            customer.Street = subject.Ulica;
            customer.CustomerName = subject.Nazwa;
            return customer;
        }
    }
}
