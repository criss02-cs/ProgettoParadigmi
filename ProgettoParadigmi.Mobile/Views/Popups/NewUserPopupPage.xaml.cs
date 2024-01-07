using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mopups.Pages;
using ProgettoParadigmi.Mobile.ViewModels.Popups;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Views.Popups;

public partial class NewUserPopupPage : PopupPage
{
    private TaskCompletionSource<RegisterDto> _taskCompletionSource;
    public Task<RegisterDto> PopupDismissedTask => _taskCompletionSource.Task; 
    public NewUserPopupPage()
    {
        InitializeComponent();
        BindingContext = new NewUserPopupPageViewModel();
    }

    private void NewUserPopupPage_OnAppearing(object? sender, EventArgs e)
    {
        base.OnAppearing();
        _taskCompletionSource = new TaskCompletionSource<RegisterDto>();
    }

    private void NewUserPopupPage_OnDisappearing(object? sender, EventArgs e)
    {
        if (BindingContext is not NewUserPopupPageViewModel vm) return;
        base.OnDisappearing();
        _taskCompletionSource.SetResult(vm.User);
    }
}