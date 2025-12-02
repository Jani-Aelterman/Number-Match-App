using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages.Popups;

public partial class ResetConfirmationPopup : Popup<bool>
{
    public ResetConfirmationPopup()
    {
        InitializeComponent();
    }

    async void Confirm_Clicked(object? sender, EventArgs e)
    {
        // close popup returning true (confirmed)
        await CloseAsync(true);
    }

    async void Cancel_Clicked(object sender, EventArgs e)
    {
        // close popup returning false (cancelled)
        await CloseAsync(false);
    }
}