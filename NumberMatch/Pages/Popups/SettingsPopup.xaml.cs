using CommunityToolkit.Maui.Views;
using NumberMatch.Helpers;
using System.Text;

namespace NumberMatch.Pages.Popups;

public partial class SettingsPopup : Popup
{
    public SettingsPopup()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        if(Preferences.ContainsKey("OledDarkmode"))
            OledDarkmode.IsSelected = Preferences.Get("OledDarkmode", false);

        if (Preferences.ContainsKey("DeveloperOptions"))
            DeveloperOptions.IsSelected = Preferences.Get("DeveloperOptions", false);
    }

    private void OledDarkmodeChanged(object sender, EventArgs e)
    {
        Preferences.Set("OledDarkmode", OledDarkmode.IsSelected);
    }

    private void DeveloperOptionsChanged(object sender, EventArgs e)
    {
        Preferences.Set("DeveloperOptions", DeveloperOptions.IsSelected);
    }
}