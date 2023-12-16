using System.Drawing;

namespace ProgettoParadigmi.Api.Utils;

public static class ColorUtils
{
    public static string GetRandomColor()
    {
        var random = new Random();
        var color = Color.FromArgb(random.Next(256), 
            random.Next(256), random.Next(256));
        return color.ToArgb().ToString();
    }
}