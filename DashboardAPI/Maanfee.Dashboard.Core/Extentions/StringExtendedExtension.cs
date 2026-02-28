namespace Maanfee.Dashboard.Core
{
    public static class StringExtendedExtension
	{
		public static T TrimStringAndCheckPersianSpecialLetter<T>(this T input) where T : class
        {
			var stringProperties = input!.GetType().GetProperties()
				.Where(p => p.PropertyType == typeof(string) && p.CanWrite);

			foreach (var stringProperty in stringProperties)
			{
                var value = stringProperty.GetValue(input, null);

                if (value != null)
                {
                    string currentValue = value.ToString() ?? string.Empty;
                    stringProperty.SetValue(input, currentValue.Trim().Replace("ي", "ی").Replace("ك", "ک"), null);
                }
            }
			return input;
		}

        public static string CheckPersianSpecialLetter(this string Value)
        {
            if (string.IsNullOrEmpty(Value))
                return Value;

            return Value.Trim().Replace("ي", "ی").Replace("ك", "ک");
        }
    }
}
