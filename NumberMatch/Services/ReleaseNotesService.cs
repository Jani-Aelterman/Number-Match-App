using System.Collections.Generic;

namespace NumberMatch.Services;

public static class ReleaseNotesService
{
    private static readonly IReadOnlyDictionary<string, string> s_releaseNotes = new Dictionary<string, string>
    {
        { "1.2", "- Added a 'help' button to show available match" +
                 "\n- Added a tutorial" +
                 "\n- Added a reset confirmation dialog" +
                 "\n- Changed settings from popup to page" +
                 "\n- Added color palette in settings to show current color scheme" +
                 "\n- Added a popup to show 'what's new' after updates" +
                 "\n- UI improvements:" +
                 "\n  * Made tekst larger and more readable" +
                 "\n  * Changed colors to be more like Material You" +
                 "\n  * Added more animations" +
                 "\n  * Removed border from popups"},
        { "1.1", "- Design changes" +
                 "\n- Performance improvements" +
                 "\n- Gameplay additions" },
        { "1.0", "- First beta release" },
        { "1.0.0", "- First beta release" }
    };

    public static string GetReleaseNotesForVersion(string version)
    {
        if (string.IsNullOrWhiteSpace(version))
            return "- Latest changes and minor fixes";

        // direct match
        if (s_releaseNotes.TryGetValue(version, out var notes))
            return notes;

        // try major.minor match (e.g. "1.2.0" -> "1.2")
        var parts = version.Split('.');
        if (parts.Length >= 2)
        {
            var majorMinor = $"{parts[0]}.{parts[1]}";
            if (s_releaseNotes.TryGetValue(majorMinor, out notes))
                return notes;
        }

        // fallback
        return "- Welcome to the latest version!\n- Minor fixes and improvements";
    }
}