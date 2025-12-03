using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages.Popups;

public partial class VersionPopup : Popup
{
    public VersionPopup()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        //Close();
        this.CloseAsync();
    }
}