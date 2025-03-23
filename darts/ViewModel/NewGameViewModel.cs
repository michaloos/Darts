using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using darts.Model;

namespace darts.ViewModel;

public class NewGameViewModel : BaseViewModel
{
    public ICommand StartNewGameCommand { get; set; }
    public ICommand AddNewUserCommand { get; set; }
    public ICommand RemoveUserCommand { get; set; }
    public bool CanStartNewGame { get; set; }
    public List<GameMode> Games { get; set; }

    private string _newUserName;

    public string NewUserName
    {
        get =>  _newUserName;
        set => SetProperty(ref _newUserName, value);
    }
    public ObservableCollection<User> Users { get; set; }
    public NewGameViewModel()
    {
        Games = GameModes.Modes;
        Users = [];
        StartNewGameCommand = new Command(StartNewGame);
        AddNewUserCommand = new Command(AddNewUser);
        RemoveUserCommand = new Command<Guid>(RemoveUser);
    }

    private async void StartNewGame()
        => await Shell.Current.GoToAsync(nameof(GamePage));

    private async void AddNewUser()
    {
        var currentPage = Application.Current?.MainPage;
        if (Users.Select(x => x.Name).Contains(NewUserName))
        {
            if (currentPage is not null)
            {
                var toast = Toast.Make("Taki użytkownik już istnieje", ToastDuration.Long);
                await toast.Show();
            }
            return;
        }
        Users.Add(new User{
            Id = Guid.NewGuid(),
            Name = NewUserName
        });
        CanStartNewGame = true;
        NewUserName = string.Empty;
    }

    private async void RemoveUser(Guid userId)
    {
        var currentPage = Application.Current?.MainPage;
        var userToRemove = Users.SingleOrDefault(x => x.Id == userId);
        if (userToRemove is not null && currentPage is not null)
        {
            var confirmation = await currentPage.DisplayAlert(
                "Potwierdzenie", 
                "Czy na pewno chcesz usunąć tego użytkownika?", 
                "Tak", 
                "Nie");
            if(confirmation)
                Users.Remove(userToRemove);
        }
    }

}