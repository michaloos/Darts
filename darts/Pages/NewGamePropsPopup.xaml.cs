using System;
using CommunityToolkit.Maui.Views;
using darts.Core.Model;
using darts.ViewModel;

namespace darts;

public partial class NewGamePropsPopup : Popup
{
    private readonly NewGamePropsPopupViewModel _viewModel;

    public NewGamePropsPopup(NewGamePropsPopupViewModel viewModel, GameMode gameMode)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
        
        _viewModel.Initialize(gameMode);
        
        _viewModel.CloseRequested += OnCloseRequested;
    }

    private void OnCloseRequested(object? sender, object result)
    {
        // Zamknij popup i zwróć wynik (konfigurację lub null)
        Close(result);
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler == null && _viewModel != null)
        {
            // Odpięcie zdarzenia przy zamknięciu popupu
            _viewModel.CloseRequested -= OnCloseRequested;
        }
    }
}