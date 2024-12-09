using Microsoft.Extensions.Logging;
using Material.Components.Maui.Extensions;
using CommunityToolkit.Maui;
using UraniumUI;
using MaterialColorUtilities.Maui;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.Maui.Platform;

namespace NumberMatch
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddMaterialIconFonts();
                })
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .UseMaterialColors(options =>
                {
                    options.FallbackSeed = 0x05affc;
                })
                .UseMaterialComponents()
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(windowsLifecycleBuilder =>
                    {
                        windowsLifecycleBuilder.OnWindowCreated(window =>
                        {
                            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                            var titleBar = appWindow.TitleBar;
                            titleBar.BackgroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                            titleBar.ButtonBackgroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                            titleBar.InactiveBackgroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                            titleBar.ButtonInactiveBackgroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                            titleBar.InactiveBackgroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                            titleBar.ForegroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                            titleBar.InactiveForegroundColor = ((Color)Application.Current.Resources["Background"]).ToWindowsColor();
                        });
                    });
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
