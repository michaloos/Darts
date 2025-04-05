using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mopups.Pages;

namespace darts;

public partial class LoadingPopup : PopupPage
{
    public LoadingPopup(string loadingMessage)
    {
        InitializeComponent();
        LoadingLabel.Text = loadingMessage;
    }
}