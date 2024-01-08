using System.Text.Json;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services;
using ProgettoParadigmi.Mobile.Services.Categorie;
using ProgettoParadigmi.Mobile.Utils;
using ProgettoParadigmi.Mobile.Views.Startup;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Startup;

public partial class LoginPageViewModel(ILoginService service, ICategorieService categorieService) : BaseViewModel
{
    [ObservableProperty] private LoginDto _loginDto = new();
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private bool _showPassword;
    private readonly ILoginService _loginService = service;

    #region Commands

    [RelayCommand]
    public void TogglePassword() => ShowPassword = !ShowPassword;

    [RelayCommand]
    public async Task TryLoadCredentials()
    {
        var str = await SecureStorage.GetAsync("cred");
        if (!string.IsNullOrEmpty(str))
        {
            var dto = JsonSerializer.Deserialize<LoginDto>(str);
            if (dto != null) LoginDto = dto;
        }
    }
    
    [RelayCommand]
    public async Task Login()
    {
        IsBusy = true;
        if (!ValidateEmail() || !ValidatePassword())
        {
            IsBusy = false;
            return;
        }
        try
        {
            var response = await _loginService.Authenticate(LoginDto);
            if (response is null)
                response = ResponseFactory
                    .CreateResponseFromResult<AuthDto>(null, false, "C'è stato un errore. Riprova fra poco");
            if (response.IsSuccess)
            {
                if (Preferences.ContainsKey(nameof(App.UserDetails)))
                {
                    Preferences.Remove(nameof(App.UserDetails));
                }
                await SecureStorage
                    .SetAsync("cred", JsonSerializer.Serialize(LoginDto));
                await SecureStorage.SetAsync("Token", response.Result.Token);
                var userDetailStr = JsonSerializer.Serialize(response.Result);
                Preferences.Set(nameof(App.UserDetails), userDetailStr);
                App.UserDetails = response.Result;
                App.Token = response.Result.Token;
                await LoadCategorie();
                IsBusy = false;
                await FlyoutManager.AddFlyoutMenusDetails();
            }
            else
            {
                IsBusy = false;
                await AppShell.Current.DisplayAlert("Errore", response.Error, "Ok");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            IsBusy = false;
            await AppShell.Current.DisplayAlert("Errore", e.Message, "Ok");
        }
    }

    [RelayCommand]
    private async Task GoToRegisterPage() => await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");

    #endregion

    private async Task LoadCategorie()
    {
        var response = await categorieService.GetByUserId(App.UserDetails.Id);
        if (response is { IsSuccess: true, Result: not null }) 
            App.Categorie = response.Result;
        else 
            await AppShell.Current.DisplayAlert("Errore", response.Error, "Ok");
    }

    private bool ValidateEmail()
    {
        ErrorMessage = "";
        if (string.IsNullOrEmpty(LoginDto.Email.Trim())
            || string.IsNullOrEmpty(LoginDto.Password.Trim()))
        {
            ErrorMessage = "Compila tutti i campi";
            return false;
        }

        if (LoginDto.Email.Contains('@')
            && LoginDto.Email.Contains('.')) return true;
        ErrorMessage = "L'email inserita non è valida";
        return false;
    }

    private bool ValidatePassword()
    {
        var regex = new Regex("^(?=.*[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{8,}$");
        if (regex.IsMatch(LoginDto.Password)) return true;
        ErrorMessage = "La password non soddisfa i requisiti";
        return false;
    }
}