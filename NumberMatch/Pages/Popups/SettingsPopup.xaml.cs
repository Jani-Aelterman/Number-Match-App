using CommunityToolkit.Maui.Views;
using NumberMatch.Helpers;
using System.Text;

namespace NumberMatch.Pages.Popups;

public partial class SettingsPopup : Popup
{
    private MainPage page;
    public SettingsPopup(MainPage mainpage)
    {
        this.page = mainpage;
        InitializeComponent();
        this.LoadSettings();
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
        page.LoadSettings();
    }

    private void DeveloperOptionsChanged(object sender, EventArgs e)
    {
        Preferences.Set("DeveloperOptions", DeveloperOptions.IsSelected);
        page.LoadSettings();
    }
}