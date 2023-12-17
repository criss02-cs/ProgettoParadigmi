using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;
[QueryProperty(nameof(Categoria), nameof(Categoria))]
public partial class HomePage : ContentPage
{
    public HomePage(HomePageViewModel vm)
    {
        InitializeComponent();
        vm.CategoriaId = Categoria;
        BindingContext = vm;
    }
    public Guid Categoria { get; set; } = Guid.Empty;
}