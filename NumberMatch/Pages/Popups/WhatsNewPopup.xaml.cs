using CommunityToolkit.Maui.Views;

namespace NumberMatch.Pages.Popups;

public partial class WhatsNewPopup : Popup
{
    public WhatsNewPopup(string releaseNotes, string version)
    {
        InitializeComponent();
        VersionLabel.Text = $"What's new — {version}";
        NotesLabel.Text = releaseNotes;
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        CloseAsync();
    }
}