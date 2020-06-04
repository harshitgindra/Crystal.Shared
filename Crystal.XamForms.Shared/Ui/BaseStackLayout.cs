using Crystal.XamForms.Shared.Abstraction;
using Xamarin.Forms;

namespace Crystal.XamForms.Shared.Ui
{
    public class BaseStackLayout : StackLayout
    {
        public BaseStackLayout(IThemeProperty themeProperty, Thickness padding = default,
            Thickness margin = default,
            View[] children = default) : this(padding, margin, children)
        {
            BackgroundColor = themeProperty.BackgroundColor;
        }

        public BaseStackLayout(Thickness padding = default,
            Thickness margin = default,
            View[] children = default)
        {
            if (padding != default) Padding = padding;

            if (margin != default) Margin = margin;

            if (children != default)
                foreach (var child in children)
                    Children.Add(child);
        }
    }
}