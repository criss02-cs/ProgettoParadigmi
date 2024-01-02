using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgettoParadigmi.Mobile.ViewModels.Dashboard;

namespace ProgettoParadigmi.Mobile.Views.Dashboard;

public partial class AddCategoryPage : ContentPage
{
    public AddCategoryPage(AddCategoryPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}