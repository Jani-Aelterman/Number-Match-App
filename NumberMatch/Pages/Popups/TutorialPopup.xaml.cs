using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages;

public partial class TutorialPopup : Popup
{
    public TutorialPopup()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}