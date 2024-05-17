using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages;

public partial class PopupPage : Popup
{
    public PopupPage(String text)
    {
        InitializeComponent();

        lbl.Text = text;

        CancelButton.IsEnabled = false;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}