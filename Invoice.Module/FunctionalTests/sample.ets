;#DropDB InvoiceEasyTest

#Application InvoiceWin
#Application InvoiceBlazor
#IfDef InvoiceWin

*Action About...
 
*Action OK

#EndIf

#IfDef InvoiceBlazor
!Action Navigation(My Details)
#EndIf
