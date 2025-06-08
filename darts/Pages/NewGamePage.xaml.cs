using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using darts.ViewModel;

namespace darts;

public partial class NewGamePage : ContentPage
{
    private readonly NewGameViewModel _viewModel;

    public NewGamePage(NewGameViewModel newGameViewModel)
    {
        InitializeComponent();
        _viewModel = newGameViewModel;
        BindingContext = newGameViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}