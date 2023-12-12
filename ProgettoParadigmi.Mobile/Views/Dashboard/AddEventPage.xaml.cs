using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

public partial class AddEventPage : ContentPage
{
    public AddEventPage(AddEventPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}