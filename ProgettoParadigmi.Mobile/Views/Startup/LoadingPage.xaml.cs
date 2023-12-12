using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Startup;

namespace ProgettoParadigmi.Mobile.Views.Startup;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}