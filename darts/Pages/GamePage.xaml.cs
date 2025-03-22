using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace darts;

public partial class GamePage : ContentPage
{
    private bool _x2IsChecked;
    private bool _x3IsChecked;
    public GamePage()
    {
        InitializeComponent();
    }

    private void PointButtonClicked(object sender, EventArgs e)
    {
        
    }
    
    private void OnToggleX2Clicked(object sender, EventArgs e)
    {
        _x2IsChecked = !_x2IsChecked;
        var button = (Button)sender;
        X2Button.BackgroundColor = _x2IsChecked ? Colors.LawnGreen : Colors.LightGray;
        X3Button.IsEnabled = !_x2IsChecked;
        Button25.IsEnabled = !_x2IsChecked && !_x3IsChecked;
        Button50.IsEnabled = !_x2IsChecked && !_x3IsChecked;
        ButtonUndo.IsEnabled = !_x2IsChecked && !_x3IsChecked;
        VisualStateManager.GoToState(button, _x2IsChecked ? "x2Checked" : "x2Unchecked");
    }
    
    private void OnToggleX3Clicked(object sender, EventArgs e)
    {
        _x3IsChecked = !_x3IsChecked;
        var button = (Button)sender;
        X3Button.BackgroundColor = _x3IsChecked ? Colors.LawnGreen : Colors.LightGray;
        X2Button.IsEnabled = !_x3IsChecked;
        Button25.IsEnabled = !_x2IsChecked && !_x3IsChecked;
        Button50.IsEnabled = !_x2IsChecked && !_x3IsChecked;
        ButtonUndo.IsEnabled = !_x2IsChecked && !_x3IsChecked;
        VisualStateManager.GoToState(button, _x3IsChecked ? "x3Checked" : "x3Unchecked");
    }
}