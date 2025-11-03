using Microsoft.Maui.Controls;

namespace NumberMatch
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register SettingsPage so Shell navigation works both in Debug en Release
            Routing.RegisterRoute(nameof(Pages.SettingsPage), typeof(Pages.SettingsPage));
        }
    }
}
