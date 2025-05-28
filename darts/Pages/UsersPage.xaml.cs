using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using darts.ViewModel;

namespace darts;

public partial class UsersPage : ContentPage
{
    public UsersPage(UsersPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        Loaded += (_, _) => viewModel.LoadUsersCommand.Execute(null);
    }
}