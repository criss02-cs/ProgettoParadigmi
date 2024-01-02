using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProgettoParadigmi.Mobile.Services.Categorie;
using ProgettoParadigmi.Mobile.Views.Dashboard;
using ProgettoParadigmi.Models.Dto;

namespace ProgettoParadigmi.Mobile.ViewModels.Dashboard;

public partial class AddCategoryPageViewModel(ICategorieService service) : BaseViewModel
{
    [ObservableProperty] private CategoriaDto _categoriaDto = new("", Guid.Empty, "");


    [RelayCommand]
    public async Task CreateCategory()
    {
        IsBusy = true;
        if (string.IsNullOrEmpty(CategoriaDto.Descrizione))
        {
            IsBusy = false;
            await Shell.Current.DisplayAlert("Errore", "La descrizione non può essere vuota", "Ok");
            return;
        }

        CategoriaDto.UserId = App.UserDetails.Id;
        var result = await service.Insert(CategoriaDto);
        IsBusy = false;
        if (result is { IsSuccess: true, Result: true })
        {
            await Shell.Current.DisplayAlert("", "Categoria creata con successo", "Ok");
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        else
        {
            await Shell.Current.DisplayAlert("", result.Error, "Ok");
        }
    }
}