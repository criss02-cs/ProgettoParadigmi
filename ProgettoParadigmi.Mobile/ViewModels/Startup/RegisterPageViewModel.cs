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

public partial class RegisterPageViewModel(ILoginService service, ICategorieService categorieService) : BaseViewModel
{
    [ObservableProperty] private RegisterDto _registerDto;
    [ObservableProperty] private string? _errorMessage;
    [ObservableProperty] private bool _showPassword;
    
    [RelayCommand]
    public void TogglePassword() => ShowPassword = !ShowPassword;

    [RelayCommand]
    public async Task Register()
    {
        IsBusy = true;
        if (!ValidateEmail() || !ValidatePassword())
        {
            ErrorMessage = "Compila tutti i campi";
            return;
        }

        try
        {
            var response = await service.Register(RegisterDto);
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
                    .SetAsync("cred", JsonSerializer.Serialize(new LoginDto
                    {
                        Email = RegisterDto.Email,
                        Password = RegisterDto.Password
                    }));
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
    private async Task GoToLoginPage() => await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    
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
        if (string.IsNullOrEmpty(RegisterDto.Email.Trim())
            || string.IsNullOrEmpty(RegisterDto.Password.Trim()))
        {
            ErrorMessage = "Compila tutti i campi";
            return false;
        }

        if (RegisterDto.Email.Contains('@')
            && RegisterDto.Email.Contains('.')) return true;
        ErrorMessage = "L'email inserita non è valida";
        return false;
    }

    private bool ValidatePassword()
    {
        var regex = new Regex("^(?=.*[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-_]).{8,}$");
        if (regex.IsMatch(RegisterDto.Password)) return true;
        ErrorMessage = "La password non soddisfa i requisiti";
        return false;
    }
}