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
        
        public static async Task HapticClick(bool hapticFeedbackEnabled)
        {
#if __MOBILE__
            if(hapticFeedbackEnabled)
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
#endif
        }

        public static async Task ErrorHaptic(bool hapticFeedbackEnabled)
        {
#if __MOBILE__
            if (hapticFeedbackEnabled)
            {
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                await Task.Delay(100);
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
                await Task.Delay(150);
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
#endif
        }
    }
}