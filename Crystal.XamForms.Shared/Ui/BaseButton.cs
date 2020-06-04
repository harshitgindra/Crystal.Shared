using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseButton : Button
    {
        public BaseButton(IThemeProperty themeProperty,
            string buttonText,
            Thickness padding = default,
            Thickness margin = default) : this(buttonText, padding, margin)
        {
            BackgroundColor = themeProperty.TextColor;
            BorderColor = themeProperty.BackgroundColor;
            TextColor = themeProperty.BackgroundColor;
            FontFamily = themeProperty.FontFamily;
        }

        public BaseButton(string text, Thickness padding = default,
            Thickness margin = default) : this()
        {
            if (!string.IsNullOrEmpty(text)) Text = text;

            if (padding != default) Padding = padding;

            if (margin != default) Margin = margin;
        }

        public BaseButton()
        {
            CornerRadius = 5;
        }
    }
}