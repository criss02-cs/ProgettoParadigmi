using ProgettoParadigmi.Mobile.Models;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Utils;

public static class GroupingExtension
{
    public static List<AdvancedEventModel> ToDayEventCollection<K, T>(this IGrouping<K, T> group)
    {
        // non andrebbe fatto, però dato che lo utilizzo solo per questo controllo il tipo
        if (group is not IGrouping<DateTime, AppuntamentoDto> grouping) return [];
        var result = grouping
            .Select(g => g.ToAdvancedEventModel())
            .ToList();
        return result;

    }
}