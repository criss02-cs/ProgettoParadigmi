using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

[QueryProperty(nameof(UserDetails), nameof(UserDetails))]
public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = new ProfilePageViewModel();
    }

    public AuthDto UserDetails
    {
        set
        {
            if (BindingContext is not ProfilePageViewModel vm) return;
            vm.UserDetails = value;
            vm.LoadProperties();
        }
    }
}