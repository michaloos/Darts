using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace darts.ViewModel;

public class NewGameViewModel : INotifyPropertyChanged
{
    public NewGameViewModel()
    {
        StartNewGameCommand = new Command(async void () =>
        {
            await Shell.Current.GoToAsync(nameof(GamePage));
        });
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public Command StartNewGameCommand { get; set; }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}