// SettingsPage.xaml.cs
using Microsoft.Maui.Controls;

namespace NumberMatch.Pages
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OledDarkmodeChanged(object sender, EventArgs e)
        {
            // Handle Oled Darkmode switch change
        }

        private void VibrationChanged(object sender, EventArgs e)
        {
            // Handle Vibration switch change
        }

        private void DeveloperOptionsChanged(object sender, EventArgs e)
        {
            // Handle Developer Options switch change
        }
    }
}