using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseFrame : Frame
    {
        public BaseFrame(IThemeProperty themeProperty, View content, Thickness padding = default,
            Thickness margin = default) : this(content, padding, margin)
        {
            BackgroundColor = themeProperty.BackgroundColor;
            BorderColor = themeProperty.TextColor;
        }

        public BaseFrame(View content, Thickness padding = default,
            Thickness margin = default)
        {
            Content = content;
            if (padding != default) Padding = padding;

            if (margin != default) Margin = margin;
        }
    }
}