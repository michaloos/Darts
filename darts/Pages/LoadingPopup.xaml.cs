using Mopups.Pages;

namespace darts;

public partial class LoadingPopup : PopupPage
{
    public LoadingPopup(string loadingMessage)
    {
        InitializeComponent();
        LoadingLabel.Text = loadingMessage;
    }

    public void UpdateMessage(string message)
    {
        MainThread.BeginInvokeOnMainThread(() => 
        {
            LoadingLabel.Text = message;
        });
    }
}