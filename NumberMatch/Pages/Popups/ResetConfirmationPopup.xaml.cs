using CommunityToolkit.Maui.Views;
using NumberMatch.Helpers;

namespace NumberMatch.Pages.Popups;

public partial class ResetConfirmationPopup : Popup<bool>
{
    private bool hapticFeedbackEnabled = true;
    
    public ResetConfirmationPopup()
    {
        InitializeComponent();

        if (Preferences.ContainsKey("Vibration"))
            hapticFeedbackEnabled = Preferences.Get("Vibration", true);
    }

    async void Confirm_Clicked(object? sender, EventArgs e)
    {
        // close popup returning true (confirmed)
        await Tools.HapticClick(hapticFeedbackEnabled);
        await CloseAsync(true);
    }

    async void Cancel_Clicked(object sender, EventArgs e)
    {
        // close popup returning false (cancelled)
        await Tools.HapticClick(hapticFeedbackEnabled);
        await CloseAsync(false);
    }
}