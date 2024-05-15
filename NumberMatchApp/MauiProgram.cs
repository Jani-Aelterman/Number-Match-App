using CommunityToolkit.Maui;
using Material.Components.Maui.Extensions;
using MaterialColorUtilities.Maui;
using Microsoft.Extensions.Logging;
using UraniumUI;
using UraniumUI.Icons.MaterialIcons;

namespace NumberMatchApp
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
                .UseMaterialColors()
                .UseMaterialComponents();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}