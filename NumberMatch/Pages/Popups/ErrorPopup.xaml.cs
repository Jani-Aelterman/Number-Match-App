using CommunityToolkit.Maui.Views;
namespace NumberMatch.Pages.Popups;

public partial class ErrorPopup : Popup
{
	public ErrorPopup(string error)
	{
		InitializeComponent();
        ErrorLabel.Text = error;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        CloseAsync();
    }
}