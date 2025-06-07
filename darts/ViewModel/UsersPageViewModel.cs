using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using darts.Core.Interface;
using darts.Core.Model;

namespace darts.ViewModel;

public class UsersPageViewModel : BaseViewModel
{
    private readonly IUnitOfWork _unitOfWork;
    public ICommand LoadUsersCommand { get; }
    public ICommand AddUserCommand { get; }
    public ICommand UpdateUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand SelectionCommand { get; }
    public ObservableCollection<User> Users { get; } = new();
    private string? _newUserName;
    private User? _selectedUser;

    public string NewUserName
    {
        get =>  _newUserName ?? "";
        set => SetProperty(ref _newUserName, value);
    }
    
    public User? SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }
    public UsersPageViewModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        LoadUsersCommand = new Command(async void () => await LoadUsers());
        AddUserCommand = new Command(async void () => await AddUserAsync());
        UpdateUserCommand = new Command(async void () => await UpdateUserAsync());
        DeleteUserCommand = new Command(async void () => await DeleteUserAsync());
        SelectionCommand = new Command<User>(user => { SelectedUser = user; });
    }

    private async Task LoadUsers()
    {
        Users.Clear();
        var users = await _unitOfWork.Users.GetAllAsync();
        foreach (var user in users)
            Users.Add(user);
    }
    
    private async Task AddUserAsync()
    {
        if (string.IsNullOrWhiteSpace(NewUserName)) return;

        var user = new User { Id = Guid.NewGuid(),  Name = NewUserName };
        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveAsync();
        await LoadUsers();
        NewUserName = string.Empty;
    }

    private async Task UpdateUserAsync()
    {
        if (SelectedUser == null || string.IsNullOrWhiteSpace(NewUserName)) return;

        SelectedUser.Name = NewUserName;
        _unitOfWork.Users.Update(SelectedUser);
        await _unitOfWork.SaveAsync();
        await LoadUsers();
        NewUserName = string.Empty;
        SelectedUser = null;
    }

    private async Task DeleteUserAsync()
    {
        if (SelectedUser == null) return;

        _unitOfWork.Users.Delete(SelectedUser);
        await _unitOfWork.SaveAsync();
        await LoadUsers();
        NewUserName = string.Empty;
        SelectedUser = null;
    }
    
}