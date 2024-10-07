using Microsoft.Extensions.Logging;
using Material.Components.Maui.Extensions;
using CommunityToolkit.Maui;
using UraniumUI;
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
                .UseMaterialComponents();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
