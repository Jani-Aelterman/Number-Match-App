using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using NumberMatch.Services;

namespace NumberMatch;

public partial class MainPage
{
    protected override void OnAppearing()
    {
        base.OnAppearing();
        // fire-and-forget: checks stored version and shows popup if needed
        _ = CheckAndShowWhatsNewAsync();
    }

    private async Task CheckAndShowWhatsNewAsync()
    {
        try
        {
            var currentVersion = $"{AppInfo.VersionString} ({AppInfo.BuildString})";
            var lastShown = Preferences.Get("LastShownVersion", string.Empty);

            if (lastShown == currentVersion)
                return;

            // Retrieve release notes for this version (centralized service)
            var notes = ReleaseNotesService.GetReleaseNotesForVersion(AppInfo.VersionString);

            // Show popup
            var popup = new Pages.Popups.WhatsNewPopup(notes, AppInfo.VersionString);
            await this.ShowPopupAsync(popup, new PopupOptions
            {
                Shape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20),
                    Stroke = dynamicBackgroundColor,
                    StrokeThickness = 0
                }
            }, CancellationToken.None);

            // Remember that we've shown this version
            Preferences.Set("LastShownVersion", currentVersion);
        }
        catch
        {
            // swallow - do not break app startup for popup failures
        }
    }
}