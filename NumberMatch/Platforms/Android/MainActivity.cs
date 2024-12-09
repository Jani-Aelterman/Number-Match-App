using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System.Threading.Tasks;
using AndroidX.Activity;

namespace NumberMatch
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static MainActivity Instance { get; private set; }

        // Set the application fullscreen
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);

            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            Instance = this;
            setupBackPressedHandling();
        }

        private void setupBackPressedHandling()
        {
            OnBackPressedDispatcher.AddCallback(new MyBackPressedCallback(true));
        }

        public override async void OnBackPressed()
        {
            var currentPage = (Microsoft.Maui.Controls.Application.Current.MainPage as Microsoft.Maui.Controls.NavigationPage)?.CurrentPage;

            if (currentPage is NumberMatch.Pages.SettingsPage)
            {
                // Navigate back to the MainPage
                await (Microsoft.Maui.Controls.Application.Current.MainPage as Microsoft.Maui.Controls.NavigationPage)?.PopAsync();
            }
            else
            {
                // If not on the SettingsPage, use the default back button behavior
                base.OnBackPressed();
            }
        }
    }

    internal class MyBackPressedCallback : OnBackPressedCallback
    {
        private bool enabled;
        public MyBackPressedCallback(bool enabled) : base(enabled)
        {
            this.enabled = enabled;
        }

        public override void HandleOnBackPressed()
        {
            if (enabled)
            {
                Console.WriteLine("========OnBackPressed get==============");

                var currentPage = (Microsoft.Maui.Controls.Application.Current.MainPage as Microsoft.Maui.Controls.NavigationPage)?.CurrentPage;

                if (currentPage is NumberMatch.Pages.SettingsPage)
                {
                    // Navigate back to the MainPage
                    (Microsoft.Maui.Controls.Application.Current.MainPage as Microsoft.Maui.Controls.NavigationPage)?.PopAsync();
                }
                else
                {
                    // If not on the SettingsPage, use the default back button behavior
                    MainActivity.Instance.OnBackPressed();
                }
            }
        }
    }
}