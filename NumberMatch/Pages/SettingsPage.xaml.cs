using HorusStudio.Maui.MaterialDesignControls;
using Microsoft.Maui.Controls;
using System;
using NumberMatch.Helpers;

namespace NumberMatch.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private MainPage page;
        private readonly Color dynamicBackgroundColor = (Color)(Application.Current.Resources["Background"] ?? Colors.Black);

        public SettingsPage(MainPage mainpage)
        {
            this.BackgroundColor = dynamicBackgroundColor;
            this.page = mainpage;
            InitializeComponent();
            this.LoadSettings();
        }

        private void LoadTheme()
        {
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
            //VibrationLabel.IsVisible = true;
            VibrationSwitch.IsVisible = true;
    #else
            //VibrationLabel.IsVisible = false;
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
            //page.LoadSettings();
        }

        private void DeveloperOptionsChanged(object sender, EventArgs e)
        {
            Preferences.Set("DeveloperOptions", DeveloperOptions.IsToggled);
            //page.LoadSettings();
        }

        private void developerBtnBackendGridClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException("Not implemented yet");
        }

        private void developerBtnRemoveRowsClicked(object sender, EventArgs e)
        {
            //page.game.RemoveEmptyRows();
        }

        private void developerBtnStageCompletionClicked(object sender, EventArgs e)
        {
            //page.game.CheckStageCompletion();
        }

        private void developerBtnRefreshGridColorsClicked(object sender, EventArgs e)
        {
            //page.RefreshGridColors();
        }

        // Back button handler: navigate back to the MainPage and refresh settings on the main page if available
        private async void BackButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Ensure main page updates (if this SettingsPage was created with a MainPage reference)
                //page?.LoadSettings();

                // Navigate back to the shell route for MainPage (absolute to ensure we return)
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Back navigation failed: {ex.Message}");
            }
        }
    }
}