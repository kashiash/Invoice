using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using Invoice.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Module.Controllers
{
    public class TransactionViewController : ViewController
    {
        SimpleAction findUnpaidInvoicesSimpleAction;

       


        public TransactionViewController()
        {
            TargetObjectType = typeof(Payment);


            findUnpaidInvoicesSimpleAction = new SimpleAction(
                this,
                $"{GetType().FullName}.{nameof(findUnpaidInvoicesSimpleAction)}",
                DevExpress.Persistent.Base.PredefinedCategory.RecordEdit)
            {
                Caption = "Find Invoices",
                ImageName = "BO_Payment",
               
                SelectionDependencyType = SelectionDependencyType.Independent,
            };


            findUnpaidInvoicesSimpleAction.Execute += FindUnpaidInvoicesSimpleAction_Execute;
        }


        private void FindUnpaidInvoicesSimpleAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach (var payment in e.SelectedObjects)
            {
                var transaction = payment as Payment;
                if (!transaction.PaymentsAssigned)
                {
                    transaction.FindInvoicesForPayment();
                }
            }

            ObjectSpace.CommitChanges();
            View.Refresh();
        }

       


    }
}
