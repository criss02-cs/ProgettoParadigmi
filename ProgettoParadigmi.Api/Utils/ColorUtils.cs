using System.Drawing;

namespace ProgettoParadigmi.Api.Utils;

public static class ColorUtils
{
    public static string GetRandomColor()
    {
        var random = new Random();
        var color = Color.FromArgb(0, random.Next(256), 
            random.Next(256), random.Next(256));
        var hexColor =
            $"#{color.A:X}{color.R:X}{color.G:X}{color.B:X}";
        return hexColor;
    }
}