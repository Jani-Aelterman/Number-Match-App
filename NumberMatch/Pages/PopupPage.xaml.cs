using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages;

public partial class PopupPage : Popup
{
    public PopupPage(String text)
    {
        InitializeComponent();

        lbl.Text = text;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}