using System;
using Plugin.Maui.Calendar.Interfaces;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Models;

public class AdvancedEventModel : AppuntamentoDto, IPersonalizableDayEvent
{
    public Color? EventIndicatorColor { get; set; }
    public Color? EventIndicatorSelectedColor { get; set; }
    public Color? EventIndicatorTextColor { get; set; }
    public Color? EventIndicatorSelectedTextColor { get; set; }
}

public static class Extension
{
    public static AdvancedEventModel ToAdvancedEventModel(this AppuntamentoDto dto)
    {
        var result = new AdvancedEventModel
        {
            Categoria = dto.Categoria,
            DataFine = dto.DataFine,
            DataInizio = dto.DataInizio,
            Descrizione = dto.Descrizione,
            EventIndicatorColor = Color.FromArgb(dto.Categoria.Color),
            EventIndicatorSelectedColor = Color.FromArgb(dto.Categoria.Color),
        };
        return result;
    }
}