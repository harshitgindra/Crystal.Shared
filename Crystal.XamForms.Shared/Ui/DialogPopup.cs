using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public static class DialogPopup
    {
        public static Task ShowError(string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                await Application.Current.MainPage.DisplayAlert(
                  title,
                  message,
                  buttonText);
            });

            if (afterHideCallback != null)
            {
                afterHideCallback();
            }
            return Task.FromResult(0);
        }

        public static Task ShowError(
            Exception error,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                await Application.Current.MainPage.DisplayAlert(
                  title,
                  error.Message,
                  buttonText);
            });

            if (afterHideCallback != null)
            {
                afterHideCallback();
            }
            return Task.FromResult(0);
        }

        public static Task ShowMessage(
            string message,
            string title)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(
                  title,
                  message,
                  "OK");
            });
            return Task.FromResult(0);
        }

        public static Task ShowMessage(
            string message,
            string title,
            string buttonText,
            Action afterHideCallback)
        {
            MainThread.BeginInvokeOnMainThread(async () => {
                await Application.Current.MainPage.DisplayAlert(
                  title,
                  message,
                  buttonText);
            });

            if (afterHideCallback != null)
            {
                afterHideCallback();
            }
            return Task.FromResult(0);
        }

        public static async Task<bool> ShowMessage(
            string message,
            string title,
            string buttonConfirmText,
            string buttonCancelText,
            Action<bool> afterHideCallback)
        {
            var result = await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                buttonConfirmText,
                buttonCancelText);

            if (afterHideCallback != null)
            {
                afterHideCallback(result);
            }
            return result;
        }

        public static async Task ShowMessageBox(
            string message,
            string title)
        {
            await Application.Current.MainPage.DisplayAlert(
                title,
                message,
                "OK");
        }
    }
}
