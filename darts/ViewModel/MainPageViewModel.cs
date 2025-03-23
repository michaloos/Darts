using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace darts.ViewModel;

public class MainPageViewModel : INotifyPropertyChanged
{
    public Command NewGamePageCommand { get; private set; }
    public Command HistoryPageCommand { get; private set; }

    public MainPageViewModel()
    {
        NewGamePageCommand = new Command(async void () =>
            await Shell.Current.GoToAsync(nameof(NewGamePage)));

        HistoryPageCommand = new Command(async void () =>
            await Shell.Current.GoToAsync(nameof(HistoryPage)));
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}