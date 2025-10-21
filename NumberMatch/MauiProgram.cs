using HorusStudio.Maui.MaterialDesignControls;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using MaterialColorUtilities.Maui;

namespace NumberMatch
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit()
                .UseMaterialColors(options =>
                {
                    options.FallbackSeed = 0x5574e1;
                })
                .UseMaterialDesignControls();

            // Register pages and MainPage so DI can resolve MainPage when constructing SettingsPage
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<Pages.SettingsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
