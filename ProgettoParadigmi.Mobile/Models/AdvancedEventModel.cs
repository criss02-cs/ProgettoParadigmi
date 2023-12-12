using System;

namespace ProgettoParadigmi.Mobile.Models;

public class AdvancedEventModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Starting { get; set; }
    public DateTime? Finishing { get; set; }
}