namespace Common.Module.Utils
{
    public class AppSettings
    {
        public static string ConnectionString
        {
            get
            {
                    //  return "Integrated Security=SSPI;Pooling=false;Data Source=.;Initial Catalog=Invoice6x";
                //@"Server=tcp:xafblazorjk.database.windows.net,1433;Initial Catalog=fleetman;Persist Security Info=False;User ID=kashiash;Password=N!....20;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                return "Server=tcp:invoicexdb.database.windows.net,1433;Initial Catalog=InvoiceDB;Persist Security Info=False;User ID=invoice.pl@outlook.com@invoicexdb;Password=N!Ezapominajka1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


            }

        }
    }
}