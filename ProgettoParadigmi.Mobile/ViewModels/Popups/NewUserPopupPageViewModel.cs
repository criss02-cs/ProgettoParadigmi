using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Popups;

public partial class NewUserPopupPageViewModel : BaseViewModel
{
    [ObservableProperty] private RegisterDto _user = new RegisterDto();
    [ObservableProperty] private bool _showPassword;

    [RelayCommand]
    private void TogglePassword() => ShowPassword = !ShowPassword;


    [RelayCommand]
    private async Task Save()
    {
        var isValidUser = ValidateUser();
        if (!isValidUser)
        {
            await Shell.Current.DisplayAlert("Errore", "Compila correttamente tutti i dati per salvare", "Ok");
        }
        else
        {
            await MopupService.Instance.PopAsync();
        }
    }
    
    
    private bool ValidateUser()
    {
        var validationContext = new ValidationContext(User);
        var validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(User, validationContext, validationResults);
    }

}