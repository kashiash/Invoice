#Application InvoiceBlazor

*FillForm
 UserName = Admin
*Action Log In

*Action Navigation(Customer)

*Action New

*FillForm
 Vat Number = 6452521870
*Action Pobierz dane z GUS

*Action Invoices.New
*FillForm
 Invoice Number = 1
 Invoice Date = 30/01/2022

*Action Invoice Items.New
*FillForm
 Product = Apple
 Quantity = 2
*Action OK

*Action Invoice Items.New
*FillForm
 Product = Orange
 Quantity = 4
*Action OK

*Action OK
*Action Save