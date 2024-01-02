using System.Globalization;

namespace ProgettoParadigmi.Mobile.Utils.Converters;

public class InitialsConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || !targetType.IsAssignableFrom(typeof(string)))
        {
            return "";
        }

        var initials = "";
        foreach (var v in values)
        {
            if (v is string s)
            {
                initials += s[..1];
            }
        }

        return initials;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}