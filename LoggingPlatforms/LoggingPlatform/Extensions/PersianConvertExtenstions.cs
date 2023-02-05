namespace LoggingPlatform.Extensions
{
	public static class PersianConvertExtenstions
    {
        public static string ToPersianString(this string Value)
        {
            return Value.Trim().Replace("ي", "ی").Replace("ك", "ک");
        }
    }
}