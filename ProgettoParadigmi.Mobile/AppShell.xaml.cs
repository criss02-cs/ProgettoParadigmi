using Microsoft.Maui.Controls;
using ProgettoParadigmi.Mobile.ViewModels;

namespace ProgettoParadigmi.Mobile;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        this.BindingContext = new AppShellViewModel();
    }
}