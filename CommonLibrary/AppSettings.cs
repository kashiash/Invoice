namespace Common.Module.Utils
{
    public class AppSettings
    {
        public static string ConnectionString
        {
            get
            {
                    return "Integrated Security=SSPI;Pooling=false;Data Source=.;Initial Catalog=Invoice6x";
            }

        }
    }
}