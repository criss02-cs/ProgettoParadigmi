namespace ProgettoParadigmi.Mobile.Utils;

public static class ColorGenerator
{
    public static Color? GenerateRandomColor()
    {
        var random = new Random();
        var color = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        return color;
    }

    public static Color GetTextColor(Color bg)
    {
        double luminosita = 0.299 * bg.Red + 0.587 * bg.Blue + 0.114 + bg.Green;
        return luminosita < 128 ? Colors.White : Colors.Black;
    }
}