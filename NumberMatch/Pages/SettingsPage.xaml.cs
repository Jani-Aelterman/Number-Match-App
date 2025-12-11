using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using HorusStudio.Maui.MaterialDesignControls;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Storage;
using NumberMatch.Helpers;
using NumberMatch.Services;
using System;
using System.Linq;
using System.Windows.Input;
//using static Android.InputMethodServices.Keyboard;
//using static Java.Util.Jar.Attributes;

namespace NumberMatch.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private bool hapticFeedbackEnabled = true;
        private readonly Color dynamicBackgroundColor = (Color)(Application.Current.Resources["Surface1"] ?? Colors.Black);

        // Command bound to the top app bar leading icon
        public ICommand GoBackCommand { get; }

        public SettingsService SettingsService { get; }

        public SettingsPage(SettingsService settingsService)
        {
            SettingsService = settingsService;

            // Bind the page to itself so XAML bindings (like LeadingIconCommand) work
            BindingContext = this;

            // Initialize go-back command: refresh main page settings then navigate back
            GoBackCommand = new Command(async () =>
            {
                try
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"GoBackCommand failed: {ex.Message}");
                }
            });

            InitializeComponent();

            LoadTheme();

            SettingsService.PropertyChanged += SettingsService_PropertyChanged;
        }

        private void SettingsService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            if (e.PropertyName == nameof(SettingsService.OledDarkmode))
            {
                LoadTheme();
            }

            if (e.PropertyName == nameof(SettingsService.Vibration))
            {
                hapticFeedbackEnabled = SettingsService.Vibration;
            }

            /*if (e.PropertyName == nameof(SettingsService.DeveloperOptions))
            {
                btnBackendGrid.IsVisible = SettingsService.DeveloperOptions;
                btnRemoveRows.IsVisible = SettingsService.DeveloperOptions;
                btnStageCompletion.IsVisible = SettingsService.DeveloperOptions;
                btnRefreshGridColors.IsVisible = SettingsService.DeveloperOptions;
            }*/
        }

        private void LoadTheme()
        {
            if (SettingsService.OledDarkmode)
                this.SetAppTheme(BackgroundColorProperty, dynamicBackgroundColor, Colors.Black);
            else
                this.SetAppTheme(BackgroundColorProperty, dynamicBackgroundColor, dynamicBackgroundColor);
        }

       /* private void LoadSettings()
        {
            if (Preferences.ContainsKey("OledDarkmode"))
                OledDarkmode.IsToggled = Preferences.Get("OledDarkmode", false);

            this.LoadTheme();

#if __MOBILE__
            if (Preferences.ContainsKey("Vibration"))
            {
                VibrationSwitch.IsToggled = Preferences.Get("Vibration", true);
                hapticFeedbackEnabled = Preferences.Get("Vibration", true);
            }

            VibrationSwitch.IsVisible = true;
    #else
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
        }*/

        /*private void OledDarkmodeChanged(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);
            Preferences.Set("OledDarkmode", OledDarkmode.IsToggled);
            page.LoadSettings();
        }*/

        /*private void VibrationChanged(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);
            Preferences.Set("Vibration", VibrationSwitch.IsToggled);
            page.LoadSettings();
        }*/

        private async void ReplayTutorialClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            var popup = new Popups.TutorialPopup();

            await this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);
        }

        /*private async void VersionInfoClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            var popup = new Popups.VersionPopup();

            await this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);
        }*/

        /*private void DeveloperOptionsChanged(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            Preferences.Set("DeveloperOptions", DeveloperOptions.IsToggled);
            page.LoadSettings();
        }*/

        private void developerBtnBackendGridClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            var popup = new Pages.Popups.ErrorPopup("This option is depreciated.");

            this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);
        }

        private void developerBtnRemoveRowsClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            //page.game.RemoveEmptyRows();
            var popup = new Pages.Popups.ErrorPopup("This option is depreciated.");

            this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);
        }

        private void developerBtnStageCompletionClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            //page.game.CheckStageCompletion();
            var popup = new Pages.Popups.ErrorPopup("This option is depreciated.");

            this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);
        }

        private void developerBtnRefreshGridColorsClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            //page.RefreshGridColors();
            var popup = new Pages.Popups.ErrorPopup("This option is depreciated.");

            this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);
        }

        // Back button handler: navigate back to the MainPage and refresh settings on the main page if available
        private async void BackButtonClicked(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            try
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Back navigation failed: {ex.Message}");
            }
        }

        // Tapped handler wired from XAML - opens the What's New popup
        private async void ShowVersionInfoTapped(object sender, EventArgs e)
        {
            Tools.HapticClick(hapticFeedbackEnabled);

            try
            {
                var version = AppInfo.VersionString;
                var notes = ReleaseNotesService.GetReleaseNotesForVersion(version);

                var popup = new Popups.WhatsNewPopup(notes, version);
                await this.ShowPopupAsync(popup, new PopupOptions
                {
                    Shape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(20),
                        Stroke = dynamicBackgroundColor,
                        StrokeThickness = 0
                    }
                }, CancellationToken.None);


            }
            catch (Exception ex)
            {
                // keep UX smooth; show a small fallback alert on failure
                await DisplayAlert("Error", "Unable to show version info: " + ex.Message, "OK");
            }
        }
    }
}