using CommunityToolkit.Maui.Views;
using darts.Core.Model;
using darts.ViewModel;

namespace darts;

public partial class NewGamePropsPopup : Popup
{
    private readonly TaskCompletionSource<object?> _tcs = new();
    public Task<object?> Result => _tcs.Task;

    public NewGamePropsPopup(NewGamePropsPopupViewModel viewModel, GameMode gameMode)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.Initialize(gameMode);

        // subskrypcja zdarzenia CloseRequested
        viewModel.CloseRequested += (s, result) =>
        {
            _tcs.SetResult(result); // ustaw wynik
            ClosePopup();           // zamknij popup
        };
    }

    private void ClosePopup()
    {
        // metoda w ContentView, zamyka popup
        // w MAUI 12 nie ma Dismiss, więc popup kończy się po zakończeniu ShowPopupAsync
        // wystarczy zakończyć task i popup zniknie
    }
}