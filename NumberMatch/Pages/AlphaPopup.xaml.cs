using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages;

public partial class AlphaPopup : Popup
{
    public AlphaPopup()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}