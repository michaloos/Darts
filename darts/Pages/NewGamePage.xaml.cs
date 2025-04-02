using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using darts.ViewModel;

namespace darts;

public partial class NewGamePage : ContentPage
{
    public NewGamePage(NewGameViewModel newGameViewModel)
    {
        InitializeComponent();
        BindingContext = newGameViewModel;
    }
}