using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using darts.ViewModel;

namespace darts;

public partial class NewGamePropsPopup : Popup
{
    public NewGamePropsPopup(NewGamePropsPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close();
    }
}