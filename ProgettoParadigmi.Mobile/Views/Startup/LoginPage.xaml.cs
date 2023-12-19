using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Startup;

namespace ProgettoParadigmi.Mobile.Views.Startup;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private void LoginPage_OnAppearing(object? sender, EventArgs e)
    {
        if (BindingContext is LoginPageViewModel vm)
        {
            vm.TryLoadCredentialsCommand.Execute(null);
        }
    }
}