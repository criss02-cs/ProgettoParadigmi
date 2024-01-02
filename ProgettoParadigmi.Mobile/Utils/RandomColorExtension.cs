namespace ProgettoParadigmi.Mobile.Utils;

public class RandomColorExtension : IMarkupExtension
{
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return ColorGenerator.GenerateRandomColor() ?? Colors.Blue;
    }
}