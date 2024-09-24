using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages;

public partial class ErrorPopup : Popup
{
    public ErrorPopup()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}