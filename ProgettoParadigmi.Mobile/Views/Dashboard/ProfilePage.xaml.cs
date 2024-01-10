using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;
using ProgettoParadigmi.Models.Dto;
using UraniumUI.Dialogs;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

[QueryProperty(nameof(UserDetails), nameof(UserDetails))]
[QueryProperty(nameof(PreviousPage), nameof(PreviousPage))]
public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        var service = Application.Current.Handler.MauiContext?.Services.GetService<IUserService>();
        var dialogService = Application.Current.Handler.MauiContext?.Services.GetService<IDialogService>();
        if (service != null) BindingContext = new ProfilePageViewModel(service, dialogService);
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