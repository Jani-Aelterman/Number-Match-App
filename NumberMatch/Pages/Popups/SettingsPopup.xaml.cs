using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Views;
using NumberMatch.Helpers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using HorusStudio.Maui.MaterialDesignControls;

namespace NumberMatch.Pages.Popups;

public partial class SettingsPopup : Popup
{
    private MainPage page;
    private readonly Color dynamicBackgroundColor = (Color)(Microsoft.Maui.Controls.Application.Current.Resources["Background"] ?? Colors.Black);
    public SettingsPopup(MainPage mainpage)
    {
        this.BackgroundColor = dynamicBackgroundColor;
        this.page = mainpage;
        InitializeComponent();
        this.LoadSettings();
        //LoadTheme();
        //PopupBackground.SetAppTheme(ColorProperty, dynamicBackgroundColor, Colors.Black);

        //BackgroundColor="{AppThemeBinding Light={StaticResource White}, Dark={StaticResource Black}}"
        //PopupBackground.SetAppThemeColor(BackgroundColorProperty, (Color)Application.Current.Resources["White"], (Color)Application.Current.Resources["Black"]);

        // Inside the SettingsPopup class constructor
        //PopupBackground.SetAppThemeColor(Frame.BackgroundColorProperty, dynamicBackgroundColor, Colors.Black);
        //PopupBackground.BackgroundColor = Application.Current.RequestedTheme == AppTheme.Dark ? (Color)Application.Current.Resources["Black"] : (Color)Application.Current.Resources["White"];
        //PopupBackground.BackgroundColor = 
        ////button.SetDynamicResource(Button.BackgroundProperty, "Primary");
    }

    private void LoadTheme()
    {
        // Fix: Use BackgroundColorProperty instead of ColorProperty
        if (Preferences.Get("OledDarkmode", false))
            this.SetAppTheme(BackgroundColorProperty, dynamicBackgroundColor, Colors.Black);
        else
            this.SetAppTheme(BackgroundColorProperty, dynamicBackgroundColor, dynamicBackgroundColor);
    }

    private void LoadSettings()
    {
        if (Preferences.ContainsKey("OledDarkmode"))
            OledDarkmode.IsToggled = Preferences.Get("OledDarkmode", false);

#if __MOBILE__
        if (Preferences.ContainsKey("Vibration"))
            VibrationSwitch.IsToggled = Preferences.Get("Vibration", true);
        VibrationLabel.IsVisible = true;
        VibrationSwitch.IsVisible = true;
#else
    VibrationLabel.IsVisible = false;
    VibrationSwitch.IsVisible = false;
#endif

        if (Preferences.ContainsKey("DeveloperOptions"))
        {
            DeveloperOptions.IsToggled = Preferences.Get("DeveloperOptions", false);
            btnBackendGrid.IsVisible = DeveloperOptions.IsToggled;
            btnRemoveRows.IsVisible = DeveloperOptions.IsToggled;
            btnStageCompletion.IsVisible = DeveloperOptions.IsToggled;
            btnRefreshGridColors.IsVisible = DeveloperOptions.IsToggled;
        }
    }

    private void OledDarkmodeChanged(object sender, EventArgs e)
    {
        Preferences.Set("OledDarkmode", OledDarkmode.IsToggled);
        page.LoadSettings();
    }

    private void VibrationChanged(object sender, EventArgs e)
    {
        Preferences.Set("Vibration", VibrationSwitch.IsToggled);
        page.LoadSettings();
    }

    private void DeveloperOptionsChanged(object sender, EventArgs e)
    {
        Preferences.Set("DeveloperOptions", DeveloperOptions.IsToggled);
        page.LoadSettings();
        //LoadTheme();
    }

    private void developerBtnBackendGridClicked(object sender, EventArgs e)
    {
        //page.ShowPopup(new Pages.Popups.gridPopup(page.game.GetGameGrid()));

        throw new NotImplementedException("Not implemented yet");
    }

    private void developerBtnRemoveRowsClicked(object sender, EventArgs e)
    {
        page.game.RemoveEmptyRows();
    }

    private void developerBtnStageCompletionClicked(object sender, EventArgs e)
    {
        page.game.CheckStageCompletion();
    }

    private void developerBtnRefreshGridColorsClicked(object sender, EventArgs e)
    {
        page.RefreshGridColors();
    }
}