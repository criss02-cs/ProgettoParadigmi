using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mopups.Pages;
using ProgettoParadigmi.Mobile.Services.Users;
using ProgettoParadigmi.Mobile.ViewModels.Popups;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.Views.Popups;

public partial class UserListPopupPage : PopupPage
{
    private TaskCompletionSource<List<UtenteDto>> _taskCompletionSource;
    public Task<List<UtenteDto>> PopupDismissedTask => _taskCompletionSource.Task; 
    
    public UserListPopupPage(IUserService service, List<UtenteDto> partecipanti)
    {
        InitializeComponent();
        BindingContext = new UserListPopupPageViewModel(service, partecipanti);
    }

    private void UserListPopupPage_OnAppearing(object? sender, EventArgs e)
    {
        base.OnAppearing();
        _taskCompletionSource = new TaskCompletionSource<List<UtenteDto>>();
    }

    private void UserListPopupPage_OnDisappearing(object? sender, EventArgs e)
    {
        if (BindingContext is not UserListPopupPageViewModel vm) return;
        base.OnDisappearing();
        _taskCompletionSource.SetResult(vm.SelectedUsers.ToList());
    }
}