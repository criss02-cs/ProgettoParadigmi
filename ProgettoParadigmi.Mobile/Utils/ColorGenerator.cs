namespace ProgettoParadigmi.Mobile.Utils;

public static class ColorGenerator
{
    public static Color? GenerateRandomColor()
    {
        var random = new Random();
        var color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        return color;
    }
}