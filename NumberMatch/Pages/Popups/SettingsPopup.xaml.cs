using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Views;
using NumberMatch.Helpers;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace NumberMatch.Pages.Popups;

public partial class SettingsPopup : Popup
{
    private MainPage page;
    private readonly Color dynamicBackgroundColor = (Color)(Microsoft.Maui.Controls.Application.Current.Resources["Background"] ?? Colors.Black);
    public SettingsPopup(MainPage mainpage)
    {
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
        if(Preferences.Get("OledDarkmode", false))
            this.SetAppTheme(ColorProperty, dynamicBackgroundColor, Colors.Black);
        else
            this.SetAppTheme(ColorProperty, dynamicBackgroundColor, dynamicBackgroundColor);
    }

    private void LoadSettings()
    {
        if(Preferences.ContainsKey("OledDarkmode"))
            OledDarkmode.IsSelected = Preferences.Get("OledDarkmode", false);

        if (Preferences.ContainsKey("DeveloperOptions"))
        {
            DeveloperOptions.IsSelected = Preferences.Get("DeveloperOptions", false);
            btnBackendGrid.IsVisible = DeveloperOptions.IsSelected;
            btnRemoveRows.IsVisible = DeveloperOptions.IsSelected;
            btnStageCompletion.IsVisible = DeveloperOptions.IsSelected;
            btnRefreshGridColors.IsVisible = DeveloperOptions.IsSelected;
        }
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
        //LoadTheme();
    }

    private void developerBtnBackendGridClicked(object sender, EventArgs e)
    {
        page.ShowPopup(new Pages.Popups.gridPopup(page.game.GetGameGrid()));
    }

    private void developerBtnRemoveRowsClicked(object sender, EventArgs e)
    {
        page.game.RemoveEmptyRowsAndShiftUp();
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