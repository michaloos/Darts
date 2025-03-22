using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace darts;

public partial class NewGamePage : ContentPage
{
    public NewGamePage()
    {
        InitializeComponent();
    }
    
    private async void StartGame(object? sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(GamePage));
}