using CommunityToolkit.Maui.Alerts;

using CommunityToolkit.Maui.Core;
using System.Threading.Tasks;

namespace NumberMatch.Helpers
{
    public static class Tools
    {
        public static async void ShowToast(string text)
        {
            double fontSize = 14;

            IToast toast = Toast.Make(text, ToastDuration.Long, fontSize);

            await toast.Show(new CancellationTokenSource().Token);
        }
    }
}