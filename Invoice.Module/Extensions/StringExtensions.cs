namespace Invoice.Module.Extensions
{
    public static class StringExtensions
    {
        public static string ChangeUnderlineToDash(this string value)
        {
            return value.Replace('_', '-');
        }
    }
}
