using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoParadigmi.Mobile.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
    public FlyoutHeaderControl()
    {
        InitializeComponent();
        if (App.UserDetails != null)
        {
            UserName.Text = $"{App.UserDetails.Nome} {App.UserDetails.Cognome}";
            Email.Text = App.UserDetails.Email;
            Role.Text = App.UserDetails.TipoUtente.ToString();
        }
    }
}