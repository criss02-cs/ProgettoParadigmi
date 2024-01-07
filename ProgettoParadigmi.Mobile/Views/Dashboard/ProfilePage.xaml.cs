using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

[QueryProperty(nameof(UserDetails), nameof(UserDetails))]
[QueryProperty(nameof(PreviousPage), nameof(PreviousPage))]
public partial class ProfilePage : ContentPage
{
    public ProfilePage(IUserService service)
    {
        InitializeComponent();
        BindingContext = new ProfilePageViewModel(service);
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

    public string PreviousPage
    {
        set
        {
            if (BindingContext is not ProfilePageViewModel vm) return;
            vm.PreviousPage = value;
        }
    }
}